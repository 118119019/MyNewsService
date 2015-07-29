using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchNewsConsole
{
    class Program
    {

        static void Main(string[] args)
        {

            while (true)
            {

                try
                {
                    Logger.WriteInfo(s.GetType().Name + "开始新闻");
                    Fetch();
                    Logger.WriteInfo(s.GetType().Name + "结束新闻");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(string.Format("抓取新闻出现异常：{0}", ex), ex);
                }


                Thread.Sleep(1000 * 60 * 60);
            }
        }

        private static void Fetch()
        {
            //更新站点分类列表
            //获取分类列表
            //根据分类列表抓取对应新闻列表
            //保存新闻列表
        }
    }
}
