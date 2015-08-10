using System;
namespace News.Model.wy163
{
    public class ArticleList
    {
        /// <summary>
        /// "B0LR1Q6O00254IU4"
        /// </summary>
        public string docid { get; set; }
        /// <summary>
        /// "http://3g.163.com/money/15/0810/15/B0LR1Q6O00254IU4.html",
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// "收评：沪指涨4.92%站上3900"
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 网易财经
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// "http://img6.cache.netease.com/3g/2015/7/14/20150714095715caf59.jpg"
        /// </summary>
        public string imgsrc { get; set; }
        public DateTime ptime { get; set; }
    }
}
