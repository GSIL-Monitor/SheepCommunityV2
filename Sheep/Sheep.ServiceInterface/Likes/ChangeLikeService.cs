﻿using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Likes
{
    /// <summary>
    ///     更改点赞的抽象服务。
    /// </summary>
    public abstract class ChangeLikeService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="like">用户身份。</param>
        protected void ResetCache(Like like)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/likes/query?parentid={0}", like.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/likes/query?parentid={0}", like.ParentId)).ToArray());
        }
    }
}