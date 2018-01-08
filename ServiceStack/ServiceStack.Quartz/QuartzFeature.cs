using System;

namespace ServiceStack.Quartz
{
    /// <summary>
    ///     设置 Quartz.Net 计划程序并扫描程序集以在默认情况下注册 IJob 实现 Quartz.net 标准应用程序配置自动使用。
    /// </summary>
    public class QuartzFeature : IPlugin, IPreInitPlugin
    {
        #region IPlugin 接口实现

        /// <inheritdoc />
        public void Register(IAppHost appHost)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPreInitPlugin 接口实现

        /// <inheritdoc />
        public void Configure(IAppHost appHost)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}