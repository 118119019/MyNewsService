using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class NewsItem
    {
        public long NewsId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int SourceSite { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateTime { get; set; }
        public string FromSite { get; set; }
        public string ImgUrl { get; set; }
        public string SourceUrl { get; set; }
        public string NewsText { get; set; }
        public string NewsContent { get; set; }
        public string ChannelName { get; set; }
    }
    public class WebNewsItem
    {
        public long NewsId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int SourceSite { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateTime { get; set; }
        public string FromSite { get; set; }
        public string NewsContent { get; set; }

    }
}

