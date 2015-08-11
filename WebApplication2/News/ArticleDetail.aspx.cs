using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonService;
using CommonService.Serilizer;
using News.DataAccess.Business;
using News.Model;

namespace WebApplication2.News
{
    public partial class ArticleDetail : Page
    {
        protected static string dbConn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
        protected WebNewsItemAccess newsItemAccess = new WebNewsItemAccess(dbConn);
        protected WebNewsItem newsItem = new WebNewsItem();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] == null)
            {
                Response.Write("无数据");
                Response.End();
                return;
            }
            else
            {
                var id = Request["id"].ToString();
                newsItem = newsItemAccess.GetContentByPk(id);
                if (newsItem == null)
                {
                    Response.Write("无数据");
                    Response.End();
                    return;
                }
                if (!string.IsNullOrEmpty(newsItem.Author))
                {
                    newsItem.Author = "作者：" + newsItem.Author + " | ";
                }

            }
        }
    }
}