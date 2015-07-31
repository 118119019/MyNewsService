using CommonService;
using CommonService.Serilizer;
using CommonUtility;
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
        protected static NewsItemAccess newsItemAccess = new NewsItemAccess(Conn);
        protected static string UnsafeTags = "head|iframe|style|script|object|embed|applet|noframes|noscript|noembed";
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
                //根据分类列表抓取对应新闻列表
                foreach (var chlCfg in chlCfgList)
                {
                    Logger.WriteInfo(string.Format("开始{0}分类抓取", chlCfg.ChannelName));
                    GetNewsDetail(site, chlCfg);
                    Logger.WriteInfo(string.Format("结束{0}分类抓取", chlCfg.ChannelName));
                }
            }




        }

        private static void GetNewsDetail(SiteConfig site, ChannelConfig chlCfg)
        {
            var url = string.Format(site.IndexUrl, chlCfg.ChannelVal);
            try
            {
                while (!string.IsNullOrEmpty(url))
                {
                    HttpResponse<xmlColumn> response = new CommonService.HttpResponse<xmlColumn>();
                    var tonghuaSunXmlColumn = response.GetFuncGetResponse(url, Serilize_Type.Xml);
                    DateTime dt = System.DateTime.Now;
                    url = tonghuaSunXmlColumn.nextPage ?? "";
                    foreach (var item in tonghuaSunXmlColumn.pageItems.item)
                    {
                        var newsParam = SqlParamHelper.GetDefaultParam(1, 10, "NewsId", true);
                        newsParam.where.where.Add(SqlParamHelper.CreateWhere(
                        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceUrl", item.url));
                        var newsItem = newsItemAccess.Load(newsParam).FirstOrDefault();
                        if (newsItem == null)
                        {
                            newsItem = new NewsItem() { NewsId = -1, SourceUrl = item.url, SourceSite = site.SiteId, Author = "" };
                        }
                        else
                        {
                            continue;
                        }
                        newsItem.Title = item.title;
                        DateTime.TryParse(item.ctime, out dt);
                        newsItem.CreateTime = dt;
                        newsItem.FromSite = item.source;
                        newsItem.ImgUrl = item.imgurl ?? "";

                        newsItem.ChannelName = tonghuaSunXmlColumn.columnName;
                        //采集内容                        
                        HtmlDocument doc = new HtmlDocument();
                        HtmlNode.ElementsFlags.Remove("option");
                        for (int i = 0; i < 5; i++)
                        {
                            try
                            {
                                doc.LoadHtml(HttpUtility.Get(newsItem.SourceUrl, Encoding.UTF8));
                                break;
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteException(string.Format("请求详情页失败，次数：{0} , url:{1}", i, newsItem.SourceUrl), ex);
                            }
                        }


                        var div = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
                        var nodes = div.SelectNodes("//*");
                        if (nodes != null)
                        {
                            nodes.ToList().ForEach(node =>
                            {
                                // 移除非安全标记
                                if (Regex.IsMatch(node.Name, UnsafeTags)) node.Remove();
                                // 筛选属性
                                node.Attributes.ToList().ForEach(attr =>
                                {
                                    // 移除脚本属性
                                    if (attr.Name.StartsWith("on")) attr.Remove();
                                    // 移除外部链接
                                    if (node.Name == "a" && attr.Name == "href")
                                    {
                                        attr.Remove();
                                    }
                                });
                            });
                        }
                        newsItem.NewsContent = div.InnerHtml;
                        //保存新闻列表
                        newsItemAccess.Save(newsItem, newsItem.NewsId.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(string.Format("分类下抓取新闻出现异常：{0}", chlCfg.ChannelName), ex);
            }
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
