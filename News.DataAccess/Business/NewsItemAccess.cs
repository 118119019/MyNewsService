using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class NewsItemAccess : BaseDataAccess<NewsItem>
    {
        public NewsItemAccess(string conn)
            : base("NewsItem", "NewsId", conn, "", null, true)
        {
        }
    }
    /// <summary>
    /// web详情页使用
    /// </summary>
    public class WebNewsItemAccess : BaseDataAccess<WebNewsItem>
    {
        public WebNewsItemAccess(string conn)
            : base("NewsItem", "NewsId", conn, "", null, true)
        {
        }
    }
}