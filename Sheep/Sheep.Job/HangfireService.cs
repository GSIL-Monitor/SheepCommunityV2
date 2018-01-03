using System;
using Microsoft.Owin.Hosting;

namespace Sheep.Job
{
    public class HangfireService
    {
        #region 静态变量

        public const string Endpoint = "http://localhost:54321";

        #endregion

        #region 属性 

        private IDisposable _host;

        #endregion

        #region 启动与停止

        public void Start()
        {
            _host = WebApp.Start<Startup>(Endpoint);
        }

        public void Stop()
        {
            _host.Dispose();
        }

        #endregion
    }
}