using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.Feedbacks
{
    /// <summary>
    ///     更改举报的抽象服务。
    /// </summary>
    public abstract class ChangeFeedbackService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="feedback">举报。</param>
        protected void ResetCache(Feedback feedback)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/feedbacks/query/byuser?userid={0}", feedback.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/feedbacks/query/byuser?userid={0}", feedback.UserId)).ToArray());
        }
    }
}