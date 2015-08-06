using CommonService;
using CommonService.Serilizer;
using CommonUtility;
using DataAccess;
using DataAccess.SqlParam;
using HtmlAgilityPack;
using News.DataAccess.Business;
using News.Model.TongHuaShun;
using News.Service.Fetch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                    Logger.WriteInfo("开始新闻抓取");
                    Fetch();
                    Logger.WriteInfo("结束新闻抓取");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(string.Format("抓取新闻出现异常：{0}", ex), ex);
                }

                Thread.Sleep(1000 * 60 * 60);
            }
        }


        protected static string Conn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
        protected static ChannelConfigAccess chlCfgAccess = new ChannelConfigAccess(Conn);
        private static void Fetch()
        {
            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            param.where.where.Add(SqlParamHelper.CreateWhere(
                PARAM_TYPE.EQUATE, LINK_TYPE.AND, "Status", "1"));
            var siteCfgAccess = new SiteConfigAccess(Conn);
            var siteList = siteCfgAccess.Load(param);
            foreach (var site in siteList)
            {
                if (site.SiteName == "同花顺")
                {
                    TongHuaSunFetchService fetchService = new TongHuaSunFetchService();
                    fetchService.Fetch(site);
                }
                if (site.SiteName == "东方财富网")
                {
                    DongfangcaifuFetchService fetchSercie = new DongfangcaifuFetchService();
                    fetchSercie.Fetch(site);
                }
            }
        }

    }
}
