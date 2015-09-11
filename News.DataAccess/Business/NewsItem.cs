using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class SeekWebNewsItem
    {
        public long NewsId { get; set; }
        /// <summary>
        /// 标题显示
        /// </summary>
        public string TitleHighLighter { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }
    }

}

