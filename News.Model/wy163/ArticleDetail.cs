using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Model.wy163
{
    public class ArticleDetail
    {
        public string body { get; set; }

        public List<ArticleDetailImg> img { get; set; }
    }
    public class ArticleDetailImg
    {
        /// <summary>
        /// "550*374"
        /// </summary>
        public string pixel { get; set; }
        public string alt { get; set; }
        public string src { get; set; }
    }
}
