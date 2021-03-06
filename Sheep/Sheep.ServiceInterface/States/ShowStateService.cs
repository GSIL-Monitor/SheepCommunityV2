﻿using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Model.Geo;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceInterface.States.Mappers;
using Sheep.ServiceModel.States;

namespace Sheep.ServiceInterface.States
{
    /// <summary>
    ///     显示一个省份服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowStateService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowStateService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个省份的校验器。
        /// </summary>
        public IValidator<StateShow> StateShowValidator { get; set; }

        /// <summary>
        ///     获取及设置省份的存储库。
        /// </summary>
        public IStateRepository StateRepo { get; set; }

        #endregion

        #region 显示一个省份

        /// <summary>
        ///     显示一个省份。
        /// </summary>
        [CacheResponse(Duration = 86400, MaxAge = 43200)]
        public async Task<object> Get(StateShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    StateShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingState = await StateRepo.GetStateAsync(request.StateId);
            if (existingState == null)
            {
                throw HttpError.NotFound(string.Format(Resources.StateNotFound, request.StateId));
            }
            var stateDto = existingState.MapToStateDto();
            return new StateShowResponse
                   {
                       State = stateDto
                   };
        }

        #endregion
    }
}