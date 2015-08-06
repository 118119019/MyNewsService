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
using System.Threading.Tasks;

namespace News.Service.Fetch
{
    public class TongHuaSunFetchService : FetchNewsManagerService
    {

        public override void UpdateSiteChl()
        {
            HttpResponse<xmlIndex> httpResponse = new HttpResponse<xmlIndex>();
            var listIndex = httpResponse.GetFuncGetResponse(site.ChannelUrl, Serilize_Type.Xml);
            foreach (var item in listIndex.item)
            {
                var chlCfg = new ChannelConfig();
                chlCfg.ChannelId = -1;
                chlCfg.ChannelName = item.name;
                chlCfg.ChannelVal = item.columnId;
                chlCfg.SiteId = site.SiteId;
                chlCfgAccess.SaveChlCfg(chlCfg);
            }
        }
        public override void GetNewsDetail(ChannelConfig chlCfg)
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
                        try
                        {
                            var div = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
                            newsItem.NewsText = div.InnerText;
                            RemoveUnsafe(div);
                            newsItem.NewsContent = div.InnerHtml;
                            //保存新闻列表
                            newsItemAccess.Save(newsItem, newsItem.NewsId.ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException("保存新闻异常", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(string.Format("分类下抓取新闻出现异常：{0}", chlCfg.ChannelName), ex);
            }
        }
        public override void Fetch(SiteConfig siteCfg)
        {
            Logger.WriteInfo("开始同花顺新闻抓取");
            base.Fetch(siteCfg);

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
            Logger.WriteInfo("结束同花顺新闻抓取");
        }
    }
}
