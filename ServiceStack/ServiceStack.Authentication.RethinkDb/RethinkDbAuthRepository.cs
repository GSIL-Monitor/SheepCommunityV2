using System;
using System.Collections.Generic;
using System.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack.Auth;
using ServiceStack.Logging;

namespace ServiceStack.Authentication.RethinkDb
{
    /// <summary>
    ///     基于RethinkDb的用户身份验证的存储库。
    /// </summary>
    public class RethinkDbAuthRepository : IUserAuthRepository, IClearable, IManageApiKeys
    {
        #region 内置计数表

        /// <summary>
        ///     内置的用户身份计数器。
        /// </summary>
        public class UserAuthCounters
        {
            public int Id { get; set; }
            public int UserAuthCounter { get; set; }
            public int UserAuthDetailsCounter { get; set; }
        }

        #endregion

        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RethinkDbAuthRepository));

        /// <summary>
        ///     RethinkDB 查询器.
        /// </summary>
        public static readonly RethinkDB R = RethinkDB.R;

        /// <summary>
        ///     用户身份的数据表名。
        /// </summary>
        private static readonly string s_UserAuthTable = typeof(UserAuth).Name;

        /// <summary>
        ///     用户身份第三方提供者的数据表名。
        /// </summary>
        private static readonly string s_UserAuthDetailsTable = typeof(UserAuthDetails).Name;

        /// <summary>
        ///     用户身份计数器的数据表名。
        /// </summary>
        private static readonly string s_UserAuthCountersTable = typeof(UserAuthCounters).Name;

        /// <summary>
        ///     API密钥的数据表名。
        /// </summary>
        private static readonly string s_ApiKeyTable = typeof(ApiKey).Name;

        #endregion

        #region 属性

        private readonly Connection _conn;
        private readonly int _shards;
        private readonly int _replicas;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="RethinkDbAuthRepository" />对象。
        /// </summary>
        /// <param name="conn">数据库连接。</param>
        /// <param name="shards">数据库分片数。</param>
        /// <param name="replicas">复制份数。</param>
        /// <param name="createMissingTables">是否创建数据表。</param>
        public RethinkDbAuthRepository(Connection conn, int shards, int replicas, bool createMissingTables)
        {
            _conn = conn;
            _shards = shards;
            _replicas = replicas;
            // 创建数据表。
            if (createMissingTables)
            {
                CreateTables();
            }
            // 检测指定的数据表是否存在。
            if (!TablesExists())
            {
                throw new InvalidOperationException(string.Format("One of the tables needed by {0} is missing. You can call {0} constructor with the parameter CreateMissingTables set to 'true'  to create the needed tables.", typeof(RethinkDbAuthRepository).Name));
            }
        }

        #endregion

        #region 数据表检测及创建

        /// <summary>
        ///     删除并重新创建数据表。
        /// </summary>
        public void DropAndReCreateTables()
        {
            _conn.CheckOpen();
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (tables.Contains(s_UserAuthTable))
            {
                R.TableDrop(s_UserAuthTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            if (tables.Contains(s_UserAuthDetailsTable))
            {
                R.TableDrop(s_UserAuthDetailsTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            if (tables.Contains(s_UserAuthCountersTable))
            {
                R.TableDrop(s_UserAuthCountersTable).RunResult(_conn).AssertNoErrors().AssertTablesDropped(1);
            }
            CreateTables();
        }

        /// <summary>
        ///     重新创建数据表。
        /// </summary>
        public void CreateTables()
        {
            _conn.CheckOpen();
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_UserAuthTable))
            {
                R.TableCreate(s_UserAuthTable).OptArg("primaryKey", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_UserAuthTable).IndexCreate("UserName").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthTable).IndexCreate("Email").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthTable).IndexCreate("DisplayName").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
            if (!tables.Contains(s_UserAuthDetailsTable))
            {
                R.TableCreate(s_UserAuthDetailsTable).OptArg("primaryKey", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_UserAuthDetailsTable).IndexCreate("UserAuthId").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthDetailsTable).IndexCreate("Provider_UserId", row => row.G("Provider").G("UserId")).RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthDetailsTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
            if (!tables.Contains(s_UserAuthCountersTable))
            {
                R.TableCreate(s_UserAuthCountersTable).OptArg("primaryKey", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                var userAuthCounters = new UserAuthCounters
                                       {
                                           Id = 0,
                                           UserAuthCounter = 0,
                                           UserAuthDetailsCounter = 0
                                       };
                R.Table(s_UserAuthCountersTable).Insert(userAuthCounters).RunResult(_conn).AssertNoErrors().AssertInserted(1);
            }
        }

        /// <summary>
        ///     检测指定的数据表是否存在。
        /// </summary>
        public bool TablesExists()
        {
            _conn.CheckOpen();
            var tableNames = new List<string>
                             {
                                 s_UserAuthTable,
                                 s_UserAuthDetailsTable,
                                 s_UserAuthCountersTable
                             };
            var tables = R.TableList().RunResult<List<string>>(_conn);
            return tables.Any(table => tableNames.Contains(table));
        }

        #endregion

        #region 加载与保存用户身份

        private void LoadUserAuthInternal(IAuthSession session, IUserAuth userAuth)
        {
            session.PopulateSession(userAuth, GetUserAuthDetails(session.UserAuthId).ConvertAll(x => (IAuthTokens) x));
        }

        private void SaveUserAuthInternal(IUserAuth userAuth)
        {
            if (userAuth.Id == default(int))
            {
                userAuth.Id = IncrementUserAuthCounter();
                R.Table(s_UserAuthTable).Insert((UserAuth) userAuth).OptArg("conflict", "error").RunResult(_conn).AssertNoErrors().AssertInserted(1);
            }
            R.Table(s_UserAuthTable).Get(userAuth.Id).Update((UserAuth) userAuth).RunResult(_conn).AssertNoErrors().AssertReplaced(1);
        }

        #endregion

        #region 增加计数器

        private int IncrementUserAuthCounter()
        {
            return IncrementCounter("UserAuthCounter").UserAuthCounter;
        }

        private int IncrementUserAuthDetailsCounter()
        {
            return IncrementCounter("UserAuthDetailsCounter").UserAuthDetailsCounter;
        }

        private UserAuthCounters IncrementCounter(string counterName)
        {
            var result = R.Table(s_UserAuthCountersTable).Get(0).Update(row => R.HashMap(counterName, row.G(counterName).Default_(0).Add(1))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<UserAuthCounters>()[0].NewValue;
        }

        #endregion

        #region IAuthRepository 接口实现

        public void LoadUserAuth(IAuthSession session, IAuthTokens tokens)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            var userAuth = GetUserAuth(session, tokens);
            LoadUserAuthInternal(session, userAuth);
        }

        public void SaveUserAuth(IAuthSession authSession)
        {
            var userAuth = !authSession.UserAuthId.IsNullOrEmpty() ? (UserAuth) GetUserAuth(authSession.UserAuthId) : authSession.ConvertTo<UserAuth>();
            if (userAuth.Id == default(int) && !authSession.UserAuthId.IsNullOrEmpty())
            {
                userAuth.Id = int.Parse(authSession.UserAuthId);
            }
            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
            {
                userAuth.CreatedDate = userAuth.ModifiedDate;
            }
            SaveUserAuthInternal(userAuth);
        }

        public List<IUserAuthDetails> GetUserAuthDetails(string userAuthId)
        {
            var userAuthDetailsList = R.Table(s_UserAuthDetailsTable).GetAll(int.Parse(userAuthId)).OptArg("index", "UserAuthId").RunResult<List<UserAuthDetails>>(_conn);
            return userAuthDetailsList.Cast<IUserAuthDetails>().ToList();
        }

        public IUserAuthDetails CreateOrMergeAuthSession(IAuthSession authSession, IAuthTokens tokens)
        {
            throw new NotImplementedException();
        }

        public IUserAuth GetUserAuth(IAuthSession authSession, IAuthTokens tokens)
        {
            if (!authSession.UserAuthId.IsNullOrEmpty())
            {
                var userAuth = GetUserAuth(authSession.UserAuthId);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (!authSession.UserAuthName.IsNullOrEmpty())
            {
                var userAuth = GetUserAuthByUserName(authSession.UserAuthName);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (tokens == null || tokens.Provider.IsNullOrEmpty() || tokens.UserId.IsNullOrEmpty())
            {
                return null;
            }
            var oAuthProviderList = R.Table(s_UserAuthDetailsTable).GetAll(R.Array(tokens.Provider, tokens.UserId)).OptArg("index", "Provider_UserId").RunResult<List<UserAuthDetails>>(_conn);
            var oAuthProvider = oAuthProviderList.FirstOrDefault();
            if (oAuthProvider != null)
            {
                return R.Table(s_UserAuthTable).Get(oAuthProvider.UserAuthId).RunResult<UserAuth>(_conn);
            }
            return null;
        }

        public IUserAuth GetUserAuthByUserName(string userNameOrEmail)
        {
            if (userNameOrEmail == null)
            {
                return null;
            }
            var isEmail = userNameOrEmail.Contains("@");
            var query = isEmail ? R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "Email") : R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "UserName");
            var userAuthList = query.RunResult<List<UserAuth>>(_conn);
            return userAuthList.FirstOrDefault();
        }

        public void SaveUserAuth(IUserAuth userAuth)
        {
            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
            {
                userAuth.CreatedDate = userAuth.ModifiedDate;
            }
            SaveUserAuthInternal(userAuth);
        }

        public bool TryAuthenticate(string userName, string password, out IUserAuth userAuth)
        {
            throw new NotImplementedException();
        }

        public bool TryAuthenticate(Dictionary<string, string> digestHeaders, string privateKey, int nonceTimeOut, string sequence, out IUserAuth userAuth)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserAuthRepository 接口实现

        public IUserAuth CreateUserAuth(IUserAuth newUser, string password)
        {
            throw new NotImplementedException();
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser)
        {
            throw new NotImplementedException();
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser, string password)
        {
            throw new NotImplementedException();
        }

        public IUserAuth GetUserAuth(string userAuthId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserAuth(string userAuthId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IClearable 接口实现

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IManageApiKeys 接口实现

        public void InitApiKeySchema()
        {
            throw new NotImplementedException();
        }

        public bool ApiKeyExists(string apiKey)
        {
            throw new NotImplementedException();
        }

        public ApiKey GetApiKey(string apiKey)
        {
            throw new NotImplementedException();
        }

        public List<ApiKey> GetUserApiKeys(string userId)
        {
            throw new NotImplementedException();
        }

        public void StoreAll(IEnumerable<ApiKey> apiKeys)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}