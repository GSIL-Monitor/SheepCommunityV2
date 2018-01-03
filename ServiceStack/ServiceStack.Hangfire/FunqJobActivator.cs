using System;
using Funq;
using Hangfire;

namespace ServiceStack.Hangfire
{
    /// <summary>
    ///     基于Funq IOC容器的任务激活器。
    /// </summary>
    public class FunqJobActivator : JobActivator
    {
        #region 属性

        private readonly Container _container;

        #endregion

        #region 构造器

        /// <summary>
        ///     初始化一个新的<see cref="FunqJobActivator" />对象。
        /// </summary>
        /// <param name="container">容器对象。</param>
        public FunqJobActivator(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = container;
        }

        #endregion

        #region JobActivator 方法重写

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return _container.TryResolve(jobType);
        }

        #endregion
    }
}