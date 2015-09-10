using CommonService;
using CommonService.Serilizer;
using CommonUtility;
using DataAccess;
using DataAccess.SqlParam;
using HtmlAgilityPack;
using News.DataAccess.Business;
using News.Model.Dongfangcaifu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Service.Fetch
{
    public class DongfangcaifuFetchService : FetchNewsManagerService
    {
        public override void UpdateSiteChl()
        {

        }
        public override void GetNewsDetail(ChannelConfig chlCfg)
        {
            var url = string.Format(site.IndexUrl + "&limit=500", chlCfg.ChannelVal);
            var cate = cateList.Find(p => p.CategoryName == "财经");
            try
            {
                HttpResponse<JSON> response = new CommonService.HttpResponse<JSON>();
                var json = response.GetFuncGetResponse(url, Serilize_Type.Json);
                //yw?encode=ywjh&limit=500
                var uri = url.Split('?')[0];
                foreach (var item in json.news)
                {
                    //var newsParam = SqlParamHelper.GetDefaultParam(1, 10, "NewsId", true);
                    //newsParam.where.where.Add(SqlParamHelper.CreateWhere(
                    //PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceUrl", item.url_m));
                    var newsItem = newsItemAccess.Find(
                        p => p.SourceUrl == item.url_m
                        );
                    if (newsItem == null)
                    {
                        newsItem = new NewsItem()
                        {
                            NewsId = -1,
                            SourceUrl = item.url_m,
                            SourceSite = site.SiteId,
                            FromSite = "东方财富网",
                            Author = ""
                        };
                    }
                    else
                    {
                        continue;
                    }
                    newsItem.CategoryId = cate.CategoryId;
                    newsItem.Title = item.title;
                    newsItem.CreateTime = item.showtime;
                    newsItem.ImgUrl = item.image ?? "";
                    newsItem.ChannelName = chlCfg.ChannelName;
                    //采集内容                        
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode.ElementsFlags.Remove("option");
                    url = uri.Replace("yw", "content?newsid=" + item.newsid.ToString());
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            HttpResponse<ContentJson> contentResponse = new CommonService.HttpResponse<ContentJson>();
                            var contentJson = contentResponse.GetFuncGetResponse(url, Serilize_Type.Json);
                            doc.LoadHtml(contentJson.body);
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
                        newsItem.NewsText = div.InnerText.Replace("<!-- EM_StockImg_Start --><!--IMG#0--><!-- EM_StockImg_End -->", "");
                        RemoveUnsafe(div);
                        newsItem.NewsContent = div.InnerHtml;
                        //保存新闻列表
                        newsItemAccess.Add(newsItem);
                        newsItemAccess.Save();
                        SaveSegMents(newsItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException("保存内容异常", ex);
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
            Logger.WriteInfo("开始东方财富网抓取");
            base.Fetch(siteCfg, newsCateList);

            //var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            //param.where.where.Add(SqlParamHelper.CreateWhere(
            //        PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SiteId", site.SiteId.ToString()));
            var chlCfgList = chlCfgAccess.FindAll(p => p.SiteId == site.SiteId);
            foreach (var chlCfg in chlCfgList)
            {
                Logger.WriteInfo(string.Format("开始{0}分类抓取", chlCfg.ChannelName));
                GetNewsDetail(chlCfg);
                Logger.WriteInfo(string.Format("结束{0}分类抓取", chlCfg.ChannelName));
            }
            CloseIndex();
            Logger.WriteInfo("结束东方财富网抓取");
        }
    }
}
