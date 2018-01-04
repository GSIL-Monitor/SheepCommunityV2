namespace Sheep.Job
{
    /// <summary>
    ///     Hangfire 后台任务服务。
    /// </summary>
    public class QuartzService
    {
        #region 静态变量

        public const string Endpoint = "http://localhost:54321";

        #endregion

        #region 属性 

        //private IDisposable _host;

        #endregion

        #region 启动与停止

        /// <summary>
        ///     服务启动。
        /// </summary>
        public void Start()
        {
            //_host = WebApp.Start<Startup>(Endpoint);
        }

        /// <summary>
        ///     服务停止。
        /// </summary>
        public void Stop()
        {
            //_host.Dispose();
        }

        #endregion
    }
}