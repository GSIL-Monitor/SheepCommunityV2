using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Recommendations
{
    /// <summary>
    ///     更改推荐 的抽象服务。
    /// </summary>
    public abstract class ChangeRecommendationService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="recommendation">推荐 。</param>
        protected void ResetCache(Recommendation recommendation)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith("date:res:/recommendations/query").ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith("res:/recommendations/query").ToArray());
        }
    }
}