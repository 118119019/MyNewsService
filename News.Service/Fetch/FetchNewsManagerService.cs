using HtmlAgilityPack;
using News.DataAccess.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace News.Service.Fetch
{
    public abstract class FetchNewsManagerService : FecthIService
    {
        public int siteid;
        public SiteConfig site;
        protected static string UnsafeTags = "head|iframe|style|script|object|embed|applet|noframes|noscript|noembed";
        protected static string dataConn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
        protected ChannelConfigAccess chlCfgAccess = new ChannelConfigAccess(dataConn);
        protected NewsItemAccess newsItemAccess = new NewsItemAccess(dataConn);

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
    }
}
