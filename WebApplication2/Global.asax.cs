using ServiceHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication2
{
    public class Global : System.Web.HttpApplication
    {
        private MySchedulerService myscheduler = MySchedulerService.GetInstance();
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            myscheduler.Run();

        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            myscheduler.ShutDown();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }
    }
}