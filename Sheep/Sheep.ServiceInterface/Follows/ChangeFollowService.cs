using System.Linq;
using ServiceStack;
using Sheep.Model.Friendship.Entities;

namespace Sheep.ServiceInterface.Follows
{
    /// <summary>
    ///     更改关注的抽象服务。
    /// </summary>
    public abstract class ChangeFollowService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="follow">用户身份。</param>
        protected void ResetCache(Follow follow)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/follows/following?followerid={0}", follow.FollowerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/follows/following?followerid={0}", follow.FollowerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/follows/basic/following?followerid={0}", follow.FollowerId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/follows/basic/following?followerid={0}", follow.FollowerId)).ToArray());

            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/follows/followed?followinguserid={0}", follow.FollowingUserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/follows/followed?followinguserid={0}", follow.FollowingUserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/follows/basic/followed?followinguserid={0}", follow.FollowingUserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/follows/basic/followed?followinguserid={0}", follow.FollowingUserId)).ToArray());
        }
    }
}