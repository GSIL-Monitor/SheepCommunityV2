using Hangfire;

namespace Sheep.Job
{
    public class HangfireService
    {
        #region 属性 

        private BackgroundJobServer _server;

        #endregion

        #region 启动与停止

        public void Start()
        {
            _server = new BackgroundJobServer();
        }

        public void Stop()
        {
            _server?.Dispose();
        }

        #endregion
    }
}