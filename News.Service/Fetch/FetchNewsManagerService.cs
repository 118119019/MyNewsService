using HtmlAgilityPack;
using News.DataAccess.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using News.Service.PanGuTool;
using News.Service.MySql;

namespace News.Service.Fetch
{
    public abstract class FetchNewsManagerService : FecthIService
    {
        public int siteid;
        public SiteConfig site;
        protected static string UnsafeTags = "head|iframe|style|script|object|embed|applet|noframes|noscript|noembed";
        protected ChannelConfigMySqlService chlCfgAccess = new ChannelConfigMySqlService();
        protected NewsItemMySqlServcie newsItemAccess = new NewsItemMySqlServcie();

        protected List<NewsCategory> cateList;



        public virtual void UpdateSiteChl()
        {

        }

        public virtual void GetNewsDetail(ChannelConfig chlCfg)
        {

        }

        public virtual void Fetch(SiteConfig siteCfg, List<NewsCategory> newsCateList)
        {
            site = siteCfg;
            cateList = newsCateList;
            UpdateSiteChl();
            Index.CreateIndex(Index.INDEX_DIR);
        }
        protected void CloseIndex()
        {

        }

        protected void RemoveUnsafe(HtmlNode div)
        {
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
        }


        protected void SaveSegMents(NewsItem newsItem)
        {
            var api = ConfigurationManager.AppSettings["WebSiteFengChi"];
            var url = api + newsItem.NewsId.ToString();
            bool IsFengChi = false;
            while (!IsFengChi)
            {
                try
                {
                    var str = CommonUtility.HttpUtility.Get(url);
                    if (str == "1")
                    {
                        IsFengChi = true;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                        IsFengChi = false;
                    }
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(5000);
                    IsFengChi = false;
                }
                 
            }
        }
    }
}
