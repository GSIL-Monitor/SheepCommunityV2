using System;
using Funq;
using Hangfire;
using Hangfire.Annotations;

namespace ServiceStack.Hangfire
{
    public static class GlobalConfigurationExtensions
    {
        #region 使用

        /// <summary>
        ///     使用基于Funq IOC容器的任务激活器。
        /// </summary>
        /// <param name="configuration">Hangfire 的全局配置。</param>
        /// <param name="container">容器对象。</param>
        /// <returns>Hangfire 的全局配置。</returns>
        public static IGlobalConfiguration<FunqJobActivator> UseFunqActivator([NotNull] this IGlobalConfiguration configuration, [NotNull] Container container)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            return configuration.UseActivator(new FunqJobActivator(container));
        }

        #endregion
    }
}