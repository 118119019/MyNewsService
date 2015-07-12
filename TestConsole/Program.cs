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


            xmlColumn xml = new xmlColumn()
            {
                columnName = "11",
                currentPage = 2,
                nextPage = "3",
                total = 1,
                pages = 4,
                pageItems = new pageItems()
                {
                    item = new List<item>() { 
                     new item (){
                      seq=1
                     }
                    }
                }
            };
            //     string str = CommonService.Serilizer.SerilizeService<xmlColumn>.CreateSerilizer(Serilize_Type.Xml).Serilize(xml);
            string url = "http://news.10jqka.com.cn/headline_mlist/1_0_0_1/";
            CommonService.HttpResponse<xmlColumn> httpResponse = new CommonService.HttpResponse<xmlColumn>();
            xml = httpResponse.PostFuncGetResponse(url, "", Serilize_Type.Xml);

            url = "http://news.10jqka.com.cn/m574677893_headline/";
            string content = CommonUtility.HttpUtility.Get(url, Encoding.UTF8);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var div = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
            string str = div.InnerText;



            Console.WriteLine(str);

            div = doc.DocumentNode.SelectSingleNode("//div[@class='title article_info']");


            var title = div.SelectSingleNode("h1");
            Console.WriteLine(title.InnerText);

            div = doc.DocumentNode.SelectSingleNode("//div[@id='extra']");
           
          //  var from = div.SelectSingleNode("span[@class='from']");

          //  Console.WriteLine(from.InnerText);


            var date = div.SelectSingleNode("span[@class='date']");

            Console.WriteLine(date.InnerText.Replace("阅读全文", ""));


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
