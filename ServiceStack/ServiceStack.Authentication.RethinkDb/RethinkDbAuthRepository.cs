using System;
using System.Collections.Generic;
using System.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack.Auth;
using ServiceStack.Logging;
using Sheep.Common.Auth;

namespace ServiceStack.Authentication.RethinkDb
{
    /// <summary>
    ///     基于RethinkDb的用户身份验证的存储库。
    /// </summary>
    public class RethinkDbAuthRepository : IUserAuthRepositoryExtended, IClearable, IManageApiKeys
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
        ///     用户身份第三方提供者明细的数据表名。
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

        private readonly IConnection _conn;
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
        public RethinkDbAuthRepository(IConnection conn, int shards, int replicas, bool createMissingTables)
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
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_UserAuthTable))
            {
                R.TableCreate(s_UserAuthTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_UserAuthTable).IndexCreate("UserName").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthTable).IndexCreate("Email").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthTable).IndexCreate("DisplayName").RunResult(_conn).AssertNoErrors();
                //R.Table(s_UserAuthTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
            if (!tables.Contains(s_UserAuthDetailsTable))
            {
                R.TableCreate(s_UserAuthDetailsTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_UserAuthDetailsTable).IndexCreate("UserAuthId").RunResult(_conn).AssertNoErrors();
                R.Table(s_UserAuthDetailsTable).IndexCreate("Provider_UserId", row => R.Array(row.G("Provider"), row.G("UserId"))).RunResult(_conn).AssertNoErrors();
                //R.Table(s_UserAuthDetailsTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
            if (!tables.Contains(s_UserAuthCountersTable))
            {
                R.TableCreate(s_UserAuthCountersTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
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

        private void InternalLoadUserAuth(IAuthSession session, IUserAuth userAuth)
        {
            session.PopulateSession(userAuth, GetUserAuthDetails(session.UserAuthId).ConvertAll(x => (IAuthTokens) x));
        }

        private void InternalSaveUserAuth(IUserAuth userAuth)
        {
            if (userAuth.Id == default(int))
            {
                userAuth.Id = IncrementUserAuthCounter();
            }
            R.Table(s_UserAuthTable).Get(userAuth.Id).Replace((UserAuth) userAuth).RunResult(_conn).AssertNoErrors();
        }

        #endregion

        #region 检测用户是否存在

        private void AssertNoExistingUser(IUserAuth newUser, IUserAuth exceptForExistingUser = null)
        {
            if (newUser.UserName != null)
            {
                var existingUser = GetUserAuthByUserName(newUser.UserName);
                if (existingUser != null && (exceptForExistingUser == null || existingUser.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(ErrorMessages.UserAlreadyExistsTemplate1, newUser.UserName.SafeInput()));
                }
            }
            if (newUser.Email != null)
            {
                var existingUser = GetUserAuthByUserName(newUser.Email);
                if (existingUser != null && (exceptForExistingUser == null || existingUser.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(ErrorMessages.EmailAlreadyExistsTemplate1, newUser.Email.SafeInput()));
                }
            }
        }

        #endregion

        #region 递增计数器

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
            InternalLoadUserAuth(session, userAuth);
        }

        public void SaveUserAuth(IAuthSession session)
        {
            var userAuth = !session.UserAuthId.IsNullOrEmpty() ? (UserAuth) GetUserAuth(session.UserAuthId) : session.ConvertTo<UserAuth>();
            if (userAuth.Id == default(int) && !session.UserAuthId.IsNullOrEmpty())
            {
                userAuth.Id = int.Parse(session.UserAuthId);
            }
            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
            {
                userAuth.CreatedDate = userAuth.ModifiedDate;
            }
            InternalSaveUserAuth(userAuth);
        }

        public List<IUserAuthDetails> GetUserAuthDetails(string userAuthId)
        {
            var userAuthDetailsList = R.Table(s_UserAuthDetailsTable).GetAll(int.Parse(userAuthId)).OptArg("index", "UserAuthId").RunResult<List<UserAuthDetails>>(_conn);
            return userAuthDetailsList.Cast<IUserAuthDetails>().ToList();
        }

        public IUserAuthDetails CreateOrMergeAuthSession(IAuthSession session, IAuthTokens tokens)
        {
            var userAuth = GetUserAuth(session, tokens) ?? new UserAuth();
            var userAuthDetails = R.Table(s_UserAuthDetailsTable).GetAll(R.Array(tokens.Provider, tokens.UserId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResult<UserAuthDetails>(_conn) ??
                                  new UserAuthDetails
                                  {
                                      Provider = tokens.Provider,
                                      UserId = tokens.UserId
                                  };
            userAuthDetails.PopulateMissing(tokens);
            userAuth.PopulateMissingExtended(userAuthDetails);
            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
            {
                userAuth.CreatedDate = userAuth.ModifiedDate;
            }
            InternalSaveUserAuth(userAuth);
            if (userAuthDetails.Id == default(int))
            {
                userAuthDetails.Id = IncrementUserAuthDetailsCounter();
            }
            userAuthDetails.UserAuthId = userAuth.Id;
            if (userAuthDetails.CreatedDate == default(DateTime))
            {
                userAuthDetails.CreatedDate = userAuth.ModifiedDate;
            }
            userAuthDetails.ModifiedDate = userAuth.ModifiedDate;
            var result = R.Table(s_UserAuthDetailsTable).Get(userAuthDetails.Id).Replace(userAuthDetails).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<UserAuthDetails>()[0].NewValue;
        }

        public IUserAuth GetUserAuth(IAuthSession session, IAuthTokens tokens)
        {
            if (!session.UserAuthId.IsNullOrEmpty())
            {
                var userAuth = GetUserAuth(session.UserAuthId);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (!session.UserAuthName.IsNullOrEmpty())
            {
                var userAuth = GetUserAuthByUserName(session.UserAuthName);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (tokens == null || tokens.Provider.IsNullOrEmpty() || tokens.UserId.IsNullOrEmpty())
            {
                return null;
            }
            var userAuthDetails = R.Table(s_UserAuthDetailsTable).GetAll(R.Array(tokens.Provider, tokens.UserId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResult<UserAuthDetails>(_conn);
            if (userAuthDetails != null)
            {
                return R.Table(s_UserAuthTable).Get(userAuthDetails.UserAuthId).RunResult<UserAuth>(_conn);
            }
            return null;
        }

        public IUserAuth GetUserAuthByUserName(string userNameOrEmail)
        {
            if (userNameOrEmail.IsNullOrEmpty())
            {
                return null;
            }
            var isEmail = userNameOrEmail.Contains("@");
            var query = isEmail ? R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "Email") : R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "UserName");
            return query.Nth(0).Default_(default(UserAuth)).RunResult<UserAuth>(_conn);
        }

        public void SaveUserAuth(IUserAuth userAuth)
        {
            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
            {
                userAuth.CreatedDate = userAuth.ModifiedDate;
            }
            InternalSaveUserAuth(userAuth);
        }

        public bool TryAuthenticate(string userNameOrEmail, string password, out IUserAuth userAuth)
        {
            userAuth = GetUserAuthByUserName(userNameOrEmail);
            if (userAuth == null)
            {
                return false;
            }
            var saltedHash = HostContext.Resolve<IHashProvider>();
            if (saltedHash.VerifyHashString(password, userAuth.PasswordHash, userAuth.Salt))
            {
                this.RecordSuccessfulLogin(userAuth);
                return true;
            }
            this.RecordInvalidLoginAttempt(userAuth);
            userAuth = null;
            return false;
        }

        public bool TryAuthenticate(Dictionary<string, string> digestHeaders, string privateKey, int nonceTimeOut, string sequence, out IUserAuth userAuth)
        {
            userAuth = GetUserAuthByUserName(digestHeaders["username"]);
            if (userAuth == null)
            {
                return false;
            }
            var digestHelper = new DigestAuthFunctions();
            if (digestHelper.ValidateResponse(digestHeaders, privateKey, nonceTimeOut, userAuth.DigestHa1Hash, sequence))
            {
                this.RecordSuccessfulLogin(userAuth);
                return true;
            }
            this.RecordInvalidLoginAttempt(userAuth);
            userAuth = null;
            return false;
        }

        #endregion

        #region IUserAuthRepository 接口实现

        public IUserAuth CreateUserAuth(IUserAuth newUser, string password)
        {
            newUser.ValidateNewUser(password);
            AssertNoExistingUser(newUser);
            var saltedHash = HostContext.Resolve<IHashProvider>();
            saltedHash.GetHashAndSaltString(password, out var hash, out var salt);
            var digestHelper = new DigestAuthFunctions();
            newUser.DigestHa1Hash = digestHelper.CreateHa1(newUser.UserName, DigestAuthProvider.Realm, password);
            newUser.PasswordHash = hash;
            newUser.Salt = salt;
            newUser.CreatedDate = DateTime.UtcNow;
            newUser.ModifiedDate = newUser.CreatedDate;
            InternalSaveUserAuth(newUser);
            return newUser;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser)
        {
            newUser.ValidateNewUser();
            AssertNoExistingUser(newUser);
            newUser.Id = existingUser.Id;
            newUser.PasswordHash = existingUser.PasswordHash;
            newUser.Salt = existingUser.Salt;
            newUser.DigestHa1Hash = existingUser.DigestHa1Hash;
            newUser.CreatedDate = existingUser.CreatedDate;
            newUser.ModifiedDate = DateTime.UtcNow;
            InternalSaveUserAuth(newUser);
            return newUser;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser, string password)
        {
            newUser.ValidateNewUser(password);
            AssertNoExistingUser(newUser, existingUser);
            var hash = existingUser.PasswordHash;
            var salt = existingUser.Salt;
            if (password != null)
            {
                var saltedHash = HostContext.Resolve<IHashProvider>();
                saltedHash.GetHashAndSaltString(password, out hash, out salt);
            }
            // If either one changes the digest hash has to be recalculated
            var digestHash = existingUser.DigestHa1Hash;
            if (password != null || existingUser.UserName != newUser.UserName)
            {
                var digestHelper = new DigestAuthFunctions();
                digestHash = digestHelper.CreateHa1(newUser.UserName, DigestAuthProvider.Realm, password);
            }
            newUser.Id = existingUser.Id;
            newUser.PasswordHash = hash;
            newUser.Salt = salt;
            newUser.DigestHa1Hash = digestHash;
            newUser.CreatedDate = existingUser.CreatedDate;
            newUser.ModifiedDate = DateTime.UtcNow;
            InternalSaveUserAuth(newUser);
            return newUser;
        }

        public IUserAuth GetUserAuth(string userAuthId)
        {
            return R.Table(s_UserAuthTable).Get(int.Parse(userAuthId)).RunResult<UserAuth>(_conn);
        }

        public void DeleteUserAuth(string userAuthId)
        {
            R.Table(s_UserAuthTable).Get(int.Parse(userAuthId)).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_UserAuthDetailsTable).GetAll(int.Parse(userAuthId)).OptArg("index", "UserAuthId").Delete().RunResult(_conn).AssertNoErrors();
        }

        #endregion

        #region IUserAuthRepositoryExtended 接口实现

        /// <summary>
        ///     根据名称获取用户身份。
        /// </summary>
        /// <param name="displayName">显示名称。</param>
        /// <returns>用户身份。</returns>
        public IUserAuth GetUserAuthByDisplayName(string displayName)
        {
            if (displayName.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_UserAuthTable).GetAll(displayName).OptArg("index", "DisplayName").Nth(0).Default_(default(UserAuth)).RunResult<UserAuth>(_conn);
        }

        /// <summary>
        ///     根据第三方提供者名称及第三方用户编号获取用户身份提供者明细。
        /// </summary>
        /// <param name="provider">第三方提供者名称。</param>
        /// <param name="userId">第三方用户编号。</param>
        /// <returns>用户身份提供者明细。</returns>
        public IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId)
        {
            if (provider.IsNullOrEmpty() || userId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResult<UserAuthDetails>(_conn);
        }

        /// <summary>
        ///     根据第三方提供者名称及第三方用户编号删除用户身份提供者明细。
        /// </summary>
        /// <param name="provider">第三方提供者名称。</param>
        /// <param name="userId">第三方用户编号。</param>
        public void DeleteUserAuthDetailsByProvider(string provider, string userId)
        {
            if (provider.IsNullOrEmpty() || userId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        #endregion

        #region IClearable 接口实现

        public void Clear()
        {
            DropAndReCreateTables();
        }

        #endregion

        #region IManageApiKeys 接口实现

        public void InitApiKeySchema()
        {
            var tables = R.TableList().RunResult<List<string>>(_conn);
            if (!tables.Contains(s_ApiKeyTable))
            {
                R.TableCreate(s_ApiKeyTable).OptArg("primary_key", "Id").OptArg("durability", Durability.Soft).OptArg("shards", _shards).OptArg("replicas", _replicas).RunResult(_conn).AssertNoErrors().AssertTablesCreated(1);
                R.Table(s_ApiKeyTable).IndexCreate("UserAuthId ").RunResult(_conn).AssertNoErrors();
                R.Table(s_ApiKeyTable).IndexWait().RunResult(_conn).AssertNoErrors();
            }
        }

        public bool ApiKeyExists(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return false;
            }
            return R.Branch(R.Table(s_ApiKeyTable).Get(apiKey) != null, true, false).RunResult<bool>(_conn);
        }

        public ApiKey GetApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return null;
            }
            return R.Table(s_ApiKeyTable).Get(apiKey).RunResult<ApiKey>(_conn);
        }

        public List<ApiKey> GetUserApiKeys(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            return R.Table(s_ApiKeyTable).GetAll(userId).OptArg("index", "UserAuthId").Filter(row => row.G("CancelledDate").Eq(null).Default_(true).And(row.G("ExpiryDate").Eq(null).Default_(true).Or(row.G("ExpiryDate").Ge(R.Now())))).RunResult<List<ApiKey>>(_conn);
        }

        public void StoreAll(IEnumerable<ApiKey> apiKeys)
        {
            if (apiKeys == null)
            {
                return;
            }
            R.Table(s_ApiKeyTable).Insert(R.Array(apiKeys.ToObjects().ToArray())).OptArg("conflict", "replace").RunResult(_conn).AssertNoErrors();
        }

        #endregion
    }
}