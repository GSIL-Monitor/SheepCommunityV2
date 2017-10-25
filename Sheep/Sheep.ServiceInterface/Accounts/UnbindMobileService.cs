//using System;
//using ServiceStack;
//using ServiceStack.Auth;
//using ServiceStack.Configuration;
//using ServiceStack.Logging;
//using ServiceStack.Web;
//using Sheep.Model.Auth.Providers;
//using Sheep.ServiceInterface.Properties;
//using Sheep.ServiceModel.Accounts;

//namespace Sheep.ServiceInterface.Accounts
//{
//    /// <summary>
//    ///     解除绑定手机号码服务接口。
//    /// </summary>
//    public class UnbindMobileService : Service
//    {
//        #region 静态变量

//        /// <summary>
//        ///     相关的日志记录器。
//        /// </summary>
//        protected static readonly ILog Log = LogManager.GetLogger(typeof(UnbindMobileService));

//        /// <summary>
//        ///     自定义校验函数。
//        /// </summary>
//        public static ValidateFn ValidateFn { get; set; }

//        #endregion

//        #region 属性 

//        /// <summary>
//        ///     获取及设置相关的应用程序设置器。
//        /// </summary>
//        public IAppSettings AppSettings { get; set; }

//        #endregion

//        #region 解除绑定手机号码

//        /// <summary>
//        ///     解除绑定手机号码。
//        /// </summary>
//        public object Delete(AccountUnbindMobile request)
//        {
//            if (!IsAuthenticated)
//            {
//                throw HttpError.Unauthorized(Resources.LoginRequired);
//            }
//            var validateResponse = ValidateFn?.Invoke(this, HttpMethods.Delete, request);
//            if (validateResponse != null)
//            {
//                return validateResponse;
//            }
//        }

//        #endregion
//    }
//}