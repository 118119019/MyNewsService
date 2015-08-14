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

using System.Configuration;

using System.Threading;


using News.Model.wy163;
using System.Collections.Generic;

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
                    //分类整理
                    //var url = "http://c.3g.163.com/nc/topicset/ios/subscribe/manage/listspecial.html";
                    //var str = HttpUtility.Get(url);
                    //str = str.Replace("{\"tList\":[", "[").Replace("]}", "]");
                    //var topicList = SerilizeService<List<Topic>>.CreateSerilizer(Serilize_Type.Json).Deserilize(str);
                    //var access = new NewsCategoryAccess(Conn);
                    //var id = 1;
                    //foreach (var item in topicList)
                    //{
                    //    if (item.tname == "原创")
                    //    {
                    //        item.tname = "网易原创";
                    //    }
                    //    var cate = new NewsCategory() { CategoryId = id, CategoryName = item.tname,  };
                    //    access.Insert(cate, cate.CategoryId.ToString());
                    //    id++;
                    //}


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
            var cateList = new NewsCategoryAccess(Conn).GetAllCate();
            foreach (var site in siteList)
            {
                if (site.SiteName == "同花顺")
                {
                    TongHuaSunFetchService fetchService = new TongHuaSunFetchService();
                    fetchService.Fetch(site, cateList);
                }
                if (site.SiteName == "东方财富网")
                {
                    DongfangcaifuFetchService fetchSercie = new DongfangcaifuFetchService();
                    fetchSercie.Fetch(site, cateList);
                }
                if (site.SiteName == "网易新闻")
                {
                    WY163FecthService fetchServcie = new WY163FecthService();
                    fetchServcie.Fetch(site, cateList);
                }
            }
        }

    }
}
