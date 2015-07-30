using CommonService;
using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using HtmlAgilityPack;
using News.DataAccess.Business;
using News.Model.TongHuaShun;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
        protected static NewsItemAccess newsItemAccess = new NewsItemAccess(Conn);

        private static void Fetch()
        {
            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            param.where.where.Add(SqlParamHelper.CreateWhere(
                PARAM_TYPE.EQUATE, LINK_TYPE.AND, "Status", "1"));
            var siteCfgAccess = new SiteConfigAccess(Conn);


            var siteList = siteCfgAccess.Load(param);
            foreach (var site in siteList)
            {
                //更新站点分类列表
                UpdateSiteChl(site);

                //获取分类列表
                param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
                param.where.where.Add(SqlParamHelper.CreateWhere(
                        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SiteId", site.SiteId.ToString()));
                var chlCfgList = chlCfgAccess.Load(param);
                foreach (var chlCfg in chlCfgList)
                {
                    var url = string.Format(site.IndexUrl, chlCfg.ChannelVal);


                    HttpResponse<xmlColumn> response = new CommonService.HttpResponse<xmlColumn>();
                    var tonghuaSunXmlColumn = response.GetFuncGetResponse(url, Serilize_Type.Xml);
                    DateTime dt = System.DateTime.Now;

                    foreach (var item in tonghuaSunXmlColumn.pageItems.item)
                    {

                        var newsParam = SqlParamHelper.GetDefaultParam(1, 10, "NewsId", true);
                        newsParam.where.where.Add(SqlParamHelper.CreateWhere(
                        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceUrl", item.url));


                        var newsItem = newsItemAccess.Load(param).FirstOrDefault();
                        if (newsItem == null)
                        {
                            newsItem = new NewsItem() { NewsId = -1, SourceUrl = item.url, SourceSite = site.SiteId };
                        }
                        newsItem.Title = item.title;
                        DateTime.TryParse(item.ctime, out dt);
                        newsItem.CreateTime = dt;
                        newsItem.FromSite = item.source;
                        newsItem.ChannelName = tonghuaSunXmlColumn.columnName;


                        //采集内容
                        //string content = CommonUtility.HttpUtility.Get(url, Encoding.UTF8);

                        //HtmlDocument doc = new HtmlDocument();
                        //doc.LoadHtml(content);
                        //var div = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
                        //string str = div.InnerText;





                        //div = doc.DocumentNode.SelectSingleNode("//div[@class='title article_info']");


                        //var title = div.SelectSingleNode("h1");
                        //Console.WriteLine(title.InnerText);

                        //div = doc.DocumentNode.SelectSingleNode("//div[@id='extra']");



                        //var date = div.SelectSingleNode("span[@class='date']");

                        //Console.WriteLine(date.InnerText.Replace("阅读全文", ""));
                    }


                }
            }


            //根据分类列表抓取对应新闻列表
            //保存新闻列表
        }


        //更新站点分类列表
        private static SqlQueryParam UpdateSiteChl(SiteConfig site)
        {
            HttpResponse<xmlIndex> httpResponse = new HttpResponse<xmlIndex>();
            var listIndex = httpResponse.GetFuncGetResponse(site.ChannelUrl, Serilize_Type.Xml);
            SqlQueryParam param = new SqlQueryParam();
            foreach (var item in listIndex.item)
            {
                param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
                param.where.where.Add(SqlParamHelper.CreateWhere(
                        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SiteId", site.SiteId.ToString()));
                param.where.where.Add(SqlParamHelper.CreateWhere(
                        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelVal", item.columnId));
                var chlCfg = chlCfgAccess.Load(param).FirstOrDefault();
                if (chlCfg == null)
                {
                    chlCfg = new ChannelConfig();
                    chlCfg.ChannelId = -1;

                }
                chlCfg.ChannelName = item.name;
                chlCfg.ChannelVal = item.columnId;
                chlCfg.SiteId = site.SiteId;
                chlCfgAccess.Save(chlCfg, chlCfg.ChannelId.ToString());
            }
            return param;
        }
    }
}
