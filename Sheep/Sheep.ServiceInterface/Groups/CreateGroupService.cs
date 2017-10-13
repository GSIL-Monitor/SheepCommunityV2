using System;
using ServiceStack;
using ServiceStack.Logging;
using Sheep.ServiceModel.Groups;

namespace Sheep.ServiceInterface.Groups
{
    /// <summary>
    ///     创建群组服务接口。
    /// </summary>
    public class CreateGroupService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateGroupService));

        #endregion

        public object Post(GroupCreate request)
        {
            var response = new GroupCreateResponse();
            try
            {
                return response;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0}", ex.GetInnerMostException().Message);
                response.ResponseStatus = ex.ToResponseStatus();
                return response;
            }
        }
    }
}