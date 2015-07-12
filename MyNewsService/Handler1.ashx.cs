using CommonService.Serilizer;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MyNewsService
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var Response = context.Response;
            if (context.Request["url"] != null)
            {
                string url = context.Request["url"];
                try
                {
                    ArticleJson articleJson = new ArticleJson();
                    string content = CommonUtility.HttpUtility.Get(url, Encoding.UTF8);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content);
                    var div = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
                    articleJson.Content = div.InnerText;
                    div = doc.DocumentNode.SelectSingleNode("//div[@class='title article_info']");
                    articleJson.Title = div.SelectSingleNode("h1").InnerText;
                    div = doc.DocumentNode.SelectSingleNode("//div[@id='extra']");
                    articleJson.date = div.SelectSingleNode("span[@class='date']").InnerText.Replace("阅读全文", "");
                    var str = SerilizeService<ArticleJson>.CreateSerilizer(Serilize_Type.Json).Serilize(articleJson);
                    Response.Output.Write(str);
                }
                catch (Exception ex)
                {
                    
                   
                }
                 
            }
            

            Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class ArticleJson
    {
        public string Title { get; set; }
        public string date { get; set; }
        public string Content { get; set; }
    }

}