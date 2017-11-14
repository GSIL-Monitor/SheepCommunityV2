using System.Linq;
using ServiceStack;
using ServiceStack.Auth;

namespace Sheep.ServiceInterface.Accounts
{
    /// <summary>
    ///     更改帐户属性的抽象服务。
    /// </summary>
    public abstract class ChangeAccountService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        protected void ResetCache(IUserAuth userAuth)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/{0}", userAuth.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/{0}", userAuth.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/{0}", userAuth.Id)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/{0}", userAuth.Id)).ToArray());
            if (!userAuth.UserName.IsNullOrEmpty())
            {
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", userAuth.UserName)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", userAuth.UserName)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", userAuth.UserName)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", userAuth.UserName)).ToArray());
            }
            if (!userAuth.Email.IsNullOrEmpty())
            {
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/show/{0}", userAuth.Email)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/show/{0}", userAuth.Email)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/users/basic/show/{0}", userAuth.Email)).ToArray());
                Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/users/basic/show/{0}", userAuth.Email)).ToArray());
            }
        }
    }
}