using CommonService.Serilizer;
using HtmlAgilityPack;
using ServiceHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using News.Service.PanGuTool;
using News.Service.MySql;
using System.Web.UI;

namespace WebApplication2
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var Response = context.Response;
            //分词
            var id = context.Request["id"];
            if (id != null)
            {
                long _id = long.Parse(id);
                var access = new NewsItemMySqlServcie();
                var newsItem = access.Find(p => p.NewsId == _id);
                if (newsItem != null)
                {
                    try
                    {
                        Index.CreateIndex(Index.INDEX_DIR);
                        Index.IndexString(Index.INDEX_DIR, newsItem.SourceUrl, newsItem.Title, newsItem.CreateTime, newsItem.NewsText, newsItem.NewsId.ToString());
                        Index.Close();
                        Response.Write("1");
                    }
                    catch (Exception ex)
                    {
                        net91com.Core.Util.LogHelper.WriteException("分词异常", ex);
                        Response.Write("0");
                    }
                }
                else
                {
                    Response.Write("1");
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




}