using DataAccess;
using DataAccess.SqlParam;
using News.DataAccess.Business;
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
                    Logger.WriteInfo(s.GetType().Name + "开始新闻抓取");
                    Fetch();
                    Logger.WriteInfo(s.GetType().Name + "结束新闻抓取");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(string.Format("抓取新闻出现异常：{0}", ex), ex);
                }


                Thread.Sleep(1000 * 60 * 60);
            }
        }


        protected static string Conn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
        private static void Fetch()
        {
            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            param.where.where.Add(SqlParamHelper.CreateWhere(
                PARAM_TYPE.EQUATE, LINK_TYPE.AND, "Status", "1"));
            var siteCfgAccess = new SiteConfigAccess(Conn);
            var siteList = siteCfgAccess.Load(param);
            foreach (var site in siteList)    //更新站点分类列表
            {
             //   CommonService.HttpResponse<list
            }

            //获取分类列表
            //根据分类列表抓取对应新闻列表
            //保存新闻列表
        }
    }
}
