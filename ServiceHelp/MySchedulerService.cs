using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelp
{
    public class MySchedulerService
    {

        private static MySchedulerService uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private MySchedulerService()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static MySchedulerService GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new MySchedulerService();
                    }
                }
            }
            return uniqueInstance;
        }

        public ISchedulerFactory sf;
        public IScheduler sched;
        private Logger log = LogManager.GetCurrentClassLogger();
        public void Run()
        {
            if (sf == null)
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";

                // set thread pool info
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "5";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                // job initialization plugin handles our xml reading, without it defaults are used
                properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
                properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";
                sf = new StdSchedulerFactory(properties);
                log.Info("------- Initialization Complete -----------");
                log.Info("------- Not Scheduling any Jobs - relying on XML definitions --");
                log.Info("------- Starting Scheduler ----------------");
            }
            sched = sf.GetScheduler();
            // start the schedule 
            sched.Start();
            log.Info("------- Started Scheduler -----------------");
        }

        public void ShutDown()
        {
            if (sched != null)
            {
                // shut down the scheduler
                log.Info("------- Shutting Down ---------------------");
                sched.Shutdown(true);
                log.Info("------- Shutdown Complete -----------------");
                SchedulerMetaData metaData = sched.GetMetaData();
                log.Info("Executed " + metaData.NumberOfJobsExecuted + " jobs.");
            }
        }
    }
    public class SimpleJob : IJob
    {
        public const string Message = "msg";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public virtual void Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            logger.Info("工作执行" + string.Format("Hello World! - {0}", DateTime.Now.ToString("r")));


            //var MailService = new MailSendFunc();
            //ResetPsdMailItem mailCfg = new ResetPsdMailItem()
            //{

            //    Email = "xcbrmbtest@126.com",
            //    Id = 1,
            //    Name = "xcbrmbtest",
            //    Psd = "a3236459",
            //    Url = ""
            //};
            //MailService.SendMail("118119019@qq.com", "发送的内容", "开发调试", mailCfg);
            string message = context.JobDetail.JobDataMap.GetString(Message);

            logger.Info(string.Format("SimpleJob: {0} executing at {1}", jobKey, DateTime.Now.ToString("r")));
            logger.Info(string.Format("SimpleJob: msg: {0}", message));
        }

    }
}
