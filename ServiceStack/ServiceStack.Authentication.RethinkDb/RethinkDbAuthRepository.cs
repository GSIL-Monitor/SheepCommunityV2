using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ServiceStack.Auth;
using ServiceStack.Authentication.RethinkDb.Properties;
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

        private async Task InternalSaveUserAuthAsync(IUserAuth userAuth)
        {
            if (userAuth.Id == default(int))
            {
                userAuth.Id = await IncrementUserAuthCounterAsync();
            }
            (await R.Table(s_UserAuthTable).Get(userAuth.Id).Replace((UserAuth) userAuth).RunResultAsync(_conn)).AssertNoErrors();
        }

        #endregion

        #region 检测用户是否存在

        private void AssertNoExistingUser(IUserAuth newUserAuth, IUserAuth exceptForExistingUser = null)
        {
            if (newUserAuth.UserName != null)
            {
                var existingUserAuth = GetUserAuthByUserName(newUserAuth.UserName);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.UserNameAlreadyExists, newUserAuth.UserName.SafeInput()));
                }
            }
            if (newUserAuth.Email != null)
            {
                var existingUserAuth = GetUserAuthByUserName(newUserAuth.Email);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.EmailAlreadyExists, newUserAuth.Email.SafeInput()));
                }
            }
            if (newUserAuth.DisplayName != null)
            {
                var existingUserAuth = GetUserAuthByDisplayName(newUserAuth.DisplayName);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.DisplayNameAlreadyExists, newUserAuth.DisplayName.SafeInput()));
                }
            }
        }

        private async Task AssertNoExistingUserAsync(IUserAuth newUserAuth, IUserAuth exceptForExistingUser = null)
        {
            if (newUserAuth.UserName != null)
            {
                var existingUserAuth = await GetUserAuthByUserNameAsync(newUserAuth.UserName);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.UserNameAlreadyExists, newUserAuth.UserName.SafeInput()));
                }
            }
            if (newUserAuth.Email != null)
            {
                var existingUserAuth = await GetUserAuthByUserNameAsync(newUserAuth.Email);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.EmailAlreadyExists, newUserAuth.Email.SafeInput()));
                }
            }
            if (newUserAuth.DisplayName != null)
            {
                var existingUserAuth = await GetUserAuthByDisplayNameAsync(newUserAuth.DisplayName);
                if (existingUserAuth != null && (exceptForExistingUser == null || existingUserAuth.Id != exceptForExistingUser.Id))
                {
                    throw new ArgumentException(string.Format(Resources.DisplayNameAlreadyExists, newUserAuth.DisplayName.SafeInput()));
                }
            }
        }

        #endregion

        #region 递增计数器

        private int IncrementUserAuthCounter()
        {
            return IncrementCounter("UserAuthCounter").UserAuthCounter;
        }

        private async Task<int> IncrementUserAuthCounterAsync()
        {
            return (await IncrementCounterAsync("UserAuthCounter")).UserAuthCounter;
        }

        private int IncrementUserAuthDetailsCounter()
        {
            return IncrementCounter("UserAuthDetailsCounter").UserAuthDetailsCounter;
        }

        private async Task<int> IncrementUserAuthDetailsCounterAsync()
        {
            return (await IncrementCounterAsync("UserAuthDetailsCounter")).UserAuthDetailsCounter;
        }

        private UserAuthCounters IncrementCounter(string counterName)
        {
            var result = R.Table(s_UserAuthCountersTable).Get(0).Update(row => R.HashMap(counterName, row.G(counterName).Default_(0).Add(1))).OptArg("return_changes", true).RunResult(_conn).AssertNoErrors();
            return result.ChangesAs<UserAuthCounters>()[0].NewValue;
        }

        private async Task<UserAuthCounters> IncrementCounterAsync(string counterName)
        {
            var result = (await R.Table(s_UserAuthCountersTable).Get(0).Update(row => R.HashMap(counterName, row.G(counterName).Default_(0).Add(1))).OptArg("return_changes", true).RunResultAsync(_conn)).AssertNoErrors();
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
            if (userAuthId.IsNullOrEmpty())
            {
                return new List<IUserAuthDetails>();
            }
            var userAuthDetailsList = R.Table(s_UserAuthDetailsTable).GetAll(userAuthId.ToInt(0)).OptArg("index", "UserAuthId").RunResult<List<UserAuthDetails>>(_conn);
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

        public IUserAuth CreateUserAuth(IUserAuth newUserAuth, string password)
        {
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser(password);
            AssertNoExistingUser(newUserAuth);
            var saltedHash = HostContext.Resolve<IHashProvider>();
            saltedHash.GetHashAndSaltString(password, out var hash, out var salt);
            var digestHelper = new DigestAuthFunctions();
            newUserAuth.DigestHa1Hash = digestHelper.CreateHa1(newUserAuth.UserName, DigestAuthProvider.Realm, password);
            newUserAuth.PasswordHash = hash;
            newUserAuth.Salt = salt;
            newUserAuth.CreatedDate = DateTime.UtcNow;
            newUserAuth.ModifiedDate = newUserAuth.CreatedDate;
            InternalSaveUserAuth(newUserAuth);
            return newUserAuth;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUserAuth, IUserAuth newUserAuth)
        {
            existingUserAuth.ThrowIfNull(nameof(existingUserAuth));
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser();
            AssertNoExistingUser(newUserAuth, existingUserAuth);
            newUserAuth.Id = existingUserAuth.Id;
            newUserAuth.PasswordHash = existingUserAuth.PasswordHash;
            newUserAuth.Salt = existingUserAuth.Salt;
            newUserAuth.DigestHa1Hash = existingUserAuth.DigestHa1Hash;
            newUserAuth.CreatedDate = existingUserAuth.CreatedDate;
            newUserAuth.ModifiedDate = DateTime.UtcNow;
            InternalSaveUserAuth(newUserAuth);
            return newUserAuth;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUserAuth, IUserAuth newUserAuth, string password)
        {
            existingUserAuth.ThrowIfNull(nameof(existingUserAuth));
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser(password);
            AssertNoExistingUser(newUserAuth, existingUserAuth);
            var hash = existingUserAuth.PasswordHash;
            var salt = existingUserAuth.Salt;
            if (password != null)
            {
                var saltedHash = HostContext.Resolve<IHashProvider>();
                saltedHash.GetHashAndSaltString(password, out hash, out salt);
            }
            // If either one changes the digest hash has to be recalculated
            var digestHash = existingUserAuth.DigestHa1Hash;
            if (password != null || existingUserAuth.UserName != newUserAuth.UserName)
            {
                var digestHelper = new DigestAuthFunctions();
                digestHash = digestHelper.CreateHa1(newUserAuth.UserName, DigestAuthProvider.Realm, password);
            }
            newUserAuth.Id = existingUserAuth.Id;
            newUserAuth.PasswordHash = hash;
            newUserAuth.Salt = salt;
            newUserAuth.DigestHa1Hash = digestHash;
            newUserAuth.CreatedDate = existingUserAuth.CreatedDate;
            newUserAuth.ModifiedDate = DateTime.UtcNow;
            InternalSaveUserAuth(newUserAuth);
            return newUserAuth;
        }

        public IUserAuth GetUserAuth(string userAuthId)
        {
            if (userAuthId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).RunResult<UserAuth>(_conn);
        }

        public void DeleteUserAuth(string userAuthId)
        {
            if (userAuthId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).Delete().RunResult(_conn).AssertNoErrors();
            R.Table(s_UserAuthDetailsTable).GetAll(userAuthId.ToInt(0)).OptArg("index", "UserAuthId").Delete().RunResult(_conn).AssertNoErrors();
        }

        #endregion

        #region IUserAuthRepositoryExtended 接口实现

        public IUserAuth GetUserAuthByDisplayName(string displayName)
        {
            if (displayName.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_UserAuthTable).GetAll(displayName).OptArg("index", "DisplayName").Nth(0).Default_(default(UserAuth)).RunResult<UserAuth>(_conn);
        }

        public List<IUserAuth> FindUserAuths(string userNameFilter, string nameFilter, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_UserAuthTable).Filter(true);
            if (!userNameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("UserName").Match(userNameFilter).Or(row.G("Email").Match(userNameFilter)));
            }
            if (!nameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("DisplayName").Match(nameFilter).Or(row.G("FullName").Match(nameFilter)));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (lockedSince.HasValue)
            {
                query = query.Filter(row => row.G("LockedDate").Gt(lockedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Meta").G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                if (orderBy == "Reputation")
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(row => row.G("Meta").G("Reputation"))) : query.OrderBy(row => row.G("Meta").G("Reputation"));
                }
                else
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
                }
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            var userAuthList = queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResult<List<UserAuth>>(_conn);
            return userAuthList.Cast<IUserAuth>().ToList();
        }

        public IUserAuthDetails GetUserAuthDetailsByProvider(string provider, string userId)
        {
            if (provider.IsNullOrEmpty() || userId.IsNullOrEmpty())
            {
                return null;
            }
            return R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResult<UserAuthDetails>(_conn);
        }

        public void DeleteUserAuthDetailsByProvider(string provider, string userId)
        {
            if (provider.IsNullOrEmpty() || userId.IsNullOrEmpty())
            {
                return;
            }
            R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Delete().RunResult(_conn).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task<IUserAuth> CreateUserAuthAsync(IUserAuth newUserAuth, string password)
        {
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser(password);
            await AssertNoExistingUserAsync(newUserAuth);
            var saltedHash = HostContext.Resolve<IHashProvider>();
            saltedHash.GetHashAndSaltString(password, out var hash, out var salt);
            var digestHelper = new DigestAuthFunctions();
            newUserAuth.DigestHa1Hash = digestHelper.CreateHa1(newUserAuth.UserName, DigestAuthProvider.Realm, password);
            newUserAuth.PasswordHash = hash;
            newUserAuth.Salt = salt;
            newUserAuth.CreatedDate = DateTime.UtcNow;
            newUserAuth.ModifiedDate = newUserAuth.CreatedDate;
            await InternalSaveUserAuthAsync(newUserAuth);
            return newUserAuth;
        }

        /// <inheritdoc />
        public async Task<IUserAuth> UpdateUserAuthAsync(IUserAuth existingUserAuth, IUserAuth newUserAuth)
        {
            existingUserAuth.ThrowIfNull(nameof(existingUserAuth));
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser();
            await AssertNoExistingUserAsync(newUserAuth, existingUserAuth);
            newUserAuth.Id = existingUserAuth.Id;
            newUserAuth.PasswordHash = existingUserAuth.PasswordHash;
            newUserAuth.Salt = existingUserAuth.Salt;
            newUserAuth.DigestHa1Hash = existingUserAuth.DigestHa1Hash;
            newUserAuth.CreatedDate = existingUserAuth.CreatedDate;
            newUserAuth.ModifiedDate = DateTime.UtcNow;
            await InternalSaveUserAuthAsync(newUserAuth);
            return newUserAuth;
        }

        /// <inheritdoc />
        public async Task<IUserAuth> UpdateUserAuthAsync(IUserAuth existingUserAuth, IUserAuth newUserAuth, string password)
        {
            existingUserAuth.ThrowIfNull(nameof(existingUserAuth));
            newUserAuth.ThrowIfNull(nameof(newUserAuth));
            newUserAuth.ValidateNewUser(password);
            await AssertNoExistingUserAsync(newUserAuth, existingUserAuth);
            var hash = existingUserAuth.PasswordHash;
            var salt = existingUserAuth.Salt;
            if (password != null)
            {
                var saltedHash = HostContext.Resolve<IHashProvider>();
                saltedHash.GetHashAndSaltString(password, out hash, out salt);
            }
            // If either one changes the digest hash has to be recalculated
            var digestHash = existingUserAuth.DigestHa1Hash;
            if (password != null || existingUserAuth.UserName != newUserAuth.UserName)
            {
                var digestHelper = new DigestAuthFunctions();
                digestHash = digestHelper.CreateHa1(newUserAuth.UserName, DigestAuthProvider.Realm, password);
            }
            newUserAuth.Id = existingUserAuth.Id;
            newUserAuth.PasswordHash = hash;
            newUserAuth.Salt = salt;
            newUserAuth.DigestHa1Hash = digestHash;
            newUserAuth.CreatedDate = existingUserAuth.CreatedDate;
            newUserAuth.ModifiedDate = DateTime.UtcNow;
            await InternalSaveUserAuthAsync(newUserAuth);
            return newUserAuth;
        }

        /// <inheritdoc />
        public async Task<IUserAuth> GetUserAuthAsync(IAuthSession session, IAuthTokens tokens)
        {
            if (!session.UserAuthId.IsNullOrEmpty())
            {
                var userAuth = await GetUserAuthAsync(session.UserAuthId);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (!session.UserAuthName.IsNullOrEmpty())
            {
                var userAuth = await GetUserAuthByUserNameAsync(session.UserAuthName);
                if (userAuth != null)
                {
                    return userAuth;
                }
            }
            if (tokens == null || tokens.Provider.IsNullOrEmpty() || tokens.UserId.IsNullOrEmpty())
            {
                return null;
            }
            var userAuthDetails = await R.Table(s_UserAuthDetailsTable).GetAll(R.Array(tokens.Provider, tokens.UserId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResultAsync<UserAuthDetails>(_conn);
            if (userAuthDetails != null)
            {
                return await R.Table(s_UserAuthTable).Get(userAuthDetails.UserAuthId).RunResultAsync<UserAuth>(_conn);
            }
            return null;
        }

        /// <inheritdoc />
        public async Task<IUserAuth> GetUserAuthAsync(string userAuthId)
        {
            return await R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).RunResultAsync<UserAuth>(_conn);
        }

        /// <inheritdoc />
        public async Task<IUserAuth> GetUserAuthByUserNameAsync(string userNameOrEmail)
        {
            var isEmail = userNameOrEmail.Contains("@");
            var query = isEmail ? R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "Email") : R.Table(s_UserAuthTable).GetAll(userNameOrEmail).OptArg("index", "UserName");
            return await query.Nth(0).Default_(default(UserAuth)).RunResultAsync<UserAuth>(_conn);
        }

        /// <inheritdoc />
        public async Task<IUserAuth> GetUserAuthByDisplayNameAsync(string displayName)
        {
            return await R.Table(s_UserAuthTable).GetAll(displayName).OptArg("index", "DisplayName").Nth(0).Default_(default(UserAuth)).RunResultAsync<UserAuth>(_conn);
        }

        /// <inheritdoc />
        public async Task<List<IUserAuth>> GetUserAuthsAsync(List<string> userAuthIds)
        {
            var userAuthList = await R.Table(s_UserAuthTable).GetAll(R.Args(userAuthIds.Select(userId => userId.ToInt(0)).ToArray())).OptArg("index", "Id").RunResultAsync<List<UserAuth>>(_conn);
            return userAuthList.Cast<IUserAuth>().ToList();
        }

        /// <inheritdoc />
        public async Task<List<IUserAuth>> FindUserAuthsAsync(List<string> userAuthIds, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_UserAuthTable).GetAll(R.Args(userAuthIds.Select(userId => userId.ToInt(0)).ToArray())).OptArg("index", "Id").Filter(true);
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (lockedSince.HasValue)
            {
                query = query.Filter(row => row.G("LockedDate").Gt(lockedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Meta").G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                if (orderBy == "Reputation")
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(row => row.G("Meta").G("Reputation"))) : query.OrderBy(row => row.G("Meta").G("Reputation"));
                }
                else
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
                }
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            var userAuthList = await queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<UserAuth>>(_conn);
            return userAuthList.Cast<IUserAuth>().ToList();
        }

        /// <inheritdoc />
        public async Task<List<IUserAuth>> FindUserAuthsAsync(string userNameFilter, string nameFilter, DateTime? createdSince, DateTime? modifiedSince, DateTime? lockedSince, string status, string orderBy, bool? descending, int? skip, int? limit)
        {
            var query = R.Table(s_UserAuthTable).Filter(true);
            if (!userNameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("UserName").Match(userNameFilter).Or(row.G("Email").Match(userNameFilter)));
            }
            if (!nameFilter.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("DisplayName").Match(nameFilter).Or(row.G("FullName").Match(nameFilter)));
            }
            if (createdSince.HasValue)
            {
                query = query.Filter(row => row.G("CreatedDate").Gt(createdSince.Value.AddSeconds(1)));
            }
            if (modifiedSince.HasValue)
            {
                query = query.Filter(row => row.G("ModifiedDate").Gt(modifiedSince.Value.AddSeconds(1)));
            }
            if (lockedSince.HasValue)
            {
                query = query.Filter(row => row.G("LockedDate").Gt(lockedSince.Value.AddSeconds(1)));
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Filter(row => row.G("Meta").G("Status").Eq(status));
            }
            OrderBy queryOrder;
            if (!orderBy.IsNullOrEmpty())
            {
                if (orderBy == "Reputation")
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(row => row.G("Meta").G("Reputation"))) : query.OrderBy(row => row.G("Meta").G("Reputation"));
                }
                else
                {
                    queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc(orderBy)) : query.OrderBy(orderBy);
                }
            }
            else
            {
                queryOrder = descending.HasValue && descending == true ? query.OrderBy(R.Desc("CreatedDate")) : query.OrderBy("CreatedDate");
            }
            var userAuthList = await queryOrder.Skip(skip ?? 0).Limit(limit ?? 10000).RunResultAsync<List<UserAuth>>(_conn);
            return userAuthList.Cast<IUserAuth>().ToList();
        }

        /// <inheritdoc />
        public async Task DeleteUserAuthAsync(string userAuthId)
        {
            (await R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).Delete().RunResultAsync(_conn)).AssertNoErrors();
            (await R.Table(s_UserAuthDetailsTable).GetAll(userAuthId.ToInt(0)).OptArg("index", "UserAuthId").Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task<IUserAuthDetails> GetUserAuthDetailsByProviderAsync(string provider, string userId)
        {
            return await R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Nth(0).Default_(default(UserAuthDetails)).RunResultAsync<UserAuthDetails>(_conn);
        }

        /// <inheritdoc />
        public async Task<List<IUserAuthDetails>> GetUserAuthDetailsAsync(string userAuthId)
        {
            var userAuthDetailsList = await R.Table(s_UserAuthDetailsTable).GetAll(userAuthId.ToInt(0)).OptArg("index", "UserAuthId").RunResultAsync<List<UserAuthDetails>>(_conn);
            return userAuthDetailsList.Cast<IUserAuthDetails>().ToList();
        }

        /// <inheritdoc />
        public async Task DeleteUserAuthDetailsByProviderAsync(string provider, string userId)
        {
            (await R.Table(s_UserAuthDetailsTable).GetAll(R.Array(provider, userId)).OptArg("index", "Provider_UserId").Delete().RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task UpdateUserAuthReputationAsync(string userAuthId, float value)
        {
            (await R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).Update(row => R.HashMap("Meta", R.HashMap("Reputation", value))).RunResultAsync(_conn)).AssertNoErrors();
        }

        /// <inheritdoc />
        public async Task UpdateUserAuthLockedDateAsync(string userAuthId, DateTime? value)
        {
            (await R.Table(s_UserAuthTable).Get(userAuthId.ToInt(0)).Update(row => R.HashMap("LockedDate", value)).RunResultAsync(_conn)).AssertNoErrors();
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