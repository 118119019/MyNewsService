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
using News.Service.MySql;

namespace WebApplication2.News
{
    public partial class ArticleDetail : Page
    {
        protected WebNewsItemMySqlServcie newsItemAccess = new WebNewsItemMySqlServcie();
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
                long _id = long.Parse(id);
                newsItem = newsItemAccess.Find(p => p.NewsId == _id);
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