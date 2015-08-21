using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonService.Serilizer;
using CommonUtility;
using DataAccess;
using DataAccess.SqlParam;
using HtmlAgilityPack;
using News.DataAccess.Business;
using News.Model.wy163;

namespace News.Service.Fetch
{
    public class WY163FecthService : FetchNewsManagerService
    {
        public override void UpdateSiteChl()
        {
            //分类整理
            var url = site.ChannelUrl;// "http://c.3g.163.com/nc/topicset/ios/subscribe/manage/listspecial.html";
            var str = HttpUtility.Get(url);
            str = str.Replace("{\"tList\":[", "[").Replace("]}", "]");
            var topicList = SerilizeService<List<Topic>>.CreateSerilizer(Serilize_Type.Json).Deserilize(str);
            var access = new NewsCategoryAccess(dataConn);
            foreach (var item in topicList)
            {
                item.tname = "网易" + item.tname;
                var chlCfg = new ChannelConfig();
                chlCfg.ChannelId = -1;
                chlCfg.ChannelName = item.tname;
                chlCfg.ChannelVal = item.tid;
                chlCfg.SiteId = site.SiteId;
                chlCfgAccess.SaveChlCfg(chlCfg);
            }
        }


        public override void GetNewsDetail(ChannelConfig chlCfg)
        {
            var url = string.Format(site.IndexUrl, chlCfg.ChannelVal);
            NewsCategory cate;
            if (chlCfg.ChannelName != "网易原创")
            {
                cate = cateList.Find(p => p.CategoryName == chlCfg.ChannelName.Replace("网易", ""));
            }
            else
            {
                cate = cateList.Find(p => p.CategoryName == chlCfg.ChannelName);
            }
            if (cate == null)
            {
                return;
            }
            try
            {

                var str = HttpUtility.Get(url);
                str = str.Replace("{\"" + chlCfg.ChannelVal + "\":[", "[").Replace("]}", "]");
                var articlList = SerilizeService<List<ArticleList>>.CreateSerilizer(Serilize_Type.Json).Deserilize(str);

                foreach (var item in articlList)
                {
                    if (string.IsNullOrEmpty(item.url))
                    {
                        continue;
                    }
                    var newsParam = SqlParamHelper.GetDefaultParam(1, 10, "NewsId", true);
                    newsParam.where.where.Add(SqlParamHelper.CreateWhere(
                    PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceUrl", item.url));
                    var newsItem = newsItemAccess.Load(newsParam).FirstOrDefault();
                    if (newsItem == null)
                    {
                        newsItem = new NewsItem()
                        {
                            NewsId = -1,
                            SourceUrl = item.url,
                            SourceSite = site.SiteId,
                            Author = ""
                        };
                    }
                    else
                    {
                        continue;
                    }
                    newsItem.CategoryId = cate.CategoryId;
                    newsItem.Title = item.title;
                    newsItem.CreateTime = item.ptime;
                    newsItem.FromSite = item.source;
                    newsItem.ImgUrl = item.imgsrc ?? "";
                    newsItem.ChannelName = chlCfg.ChannelName;
                    //采集内容          http://c.3g.163.com/nc/article/B0DQ29J400031H2L/full.html               
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode.ElementsFlags.Remove("option");
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            var sourceUrl = string.Format(site.DetailUrl, item.docid);
                            str = HttpUtility.Get(sourceUrl);
                            str = str.Replace("{\"" + item.docid + "\":{", "{").Replace("}}", "}");

                            var article = SerilizeService<ArticleDetail>.CreateSerilizer(Serilize_Type.Json).Deserilize(str);
                            if (article.img != null)
                            {
                                foreach (var img in article.img)
                                {
                                    article.body =
                                       string.Format("<p><img src=\"{0}\" itemprop=\"image\" alt=\"\" >{1}</ p >", img.src, img.alt)
                                        + article.body;
                                }
                            }

                            doc.LoadHtml(article.body);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException(string.Format("请求详情页失败，次数：{0} , url:{1}", i, newsItem.SourceUrl), ex);
                        }
                    }
                    try
                    {
                        var div = doc.DocumentNode;
                        if (string.IsNullOrEmpty(div.InnerText))
                        {
                            continue;
                        }
                        var matchstr = (string)div.InnerText.ToString().Clone();
                        MatchCollection matchList = Regex.Matches(matchstr, @"<!--\S*-->");
                        foreach (Match match in matchList)
                        {
                            matchstr = matchstr.Replace(match.Value, "");
                        }
                        newsItem.NewsText = matchstr;
                        RemoveUnsafe(div);
                        newsItem.NewsContent = div.InnerHtml;
                        //保存新闻列表
                        newsItemAccess.Save(newsItem, newsItem.NewsId.ToString());
                        SaveSegMents(newsItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException("保存新闻异常", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(string.Format("分类下抓取新闻出现异常：{0}", chlCfg.ChannelName), ex);
            }
        }
        public override void Fetch(SiteConfig siteCfg, List<NewsCategory> newsCateList)
        {
            Logger.WriteInfo("开始网易新闻抓取");
            base.Fetch(siteCfg, newsCateList);

            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            param.where.where.Add(SqlParamHelper.CreateWhere(
                    PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SiteId", site.SiteId.ToString()));

            var chlCfgList = chlCfgAccess.Load(param);
            foreach (var chlCfg in chlCfgList)
            {
                Logger.WriteInfo(string.Format("开始{0}分类抓取", chlCfg.ChannelName));
                GetNewsDetail(chlCfg);
                Logger.WriteInfo(string.Format("结束{0}分类抓取", chlCfg.ChannelName));
            }
            CloseIndex();
            Logger.WriteInfo("结束网易新闻抓取");
        }
    }
}
