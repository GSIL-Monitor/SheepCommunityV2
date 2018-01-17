using System.Linq;
using ServiceStack;
using Sheep.Model.Content.Entities;

namespace Sheep.ServiceInterface.AbuseReports
{
    /// <summary>
    ///     更改举报的抽象服务。
    /// </summary>
    public abstract class ChangeAbuseReportService : Service
    {
        /// <summary>
        ///     重置缓存。
        /// </summary>
        /// <param name="report">举报。</param>
        protected void ResetCache(AbuseReport report)
        {
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/abusereports/query/byparent?parentid={0}", report.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/abusereports/query/byparent?parentid={0}", report.ParentId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("date:res:/abusereports/query/byuser?userid={0}", report.UserId)).ToArray());
            Request.RemoveFromCache(Cache, Cache.GetKeysStartingWith(string.Format("res:/abusereports/query/byuser?userid={0}", report.UserId)).ToArray());
        }
    }
}