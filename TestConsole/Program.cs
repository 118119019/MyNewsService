using CommonService.Serilizer;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {


           
            Console.ReadKey();
        }

        public class ArticleJson
        {
            public string Title { get; set; }
            public string From { get; set; }
            public string Content { get; set; }
        }

        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }

    public class xmlColumn
    {

        public string columnName { get; set; }

        public int total { get; set; }

        public int pages { get; set; }

        public string nextPage { get; set; }

        public int currentPage { get; set; }
        [XmlElement]
        public pageItems pageItems { get; set; }
    }
    public class pageItems
    {
        [XmlElement]
        public List<item> item { get; set; }
    }
    public class item
    {
        public long seq { get; set; }
        public string title { get; set; }
        public string ctime { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string hot { get; set; }
        public string imgurl { get; set; }
    }
}
