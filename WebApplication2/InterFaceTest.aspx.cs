using CommonService;
using CommonService.Serilizer;
using News.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using News.Service.PanGuTool;
using News.DataAccess.Business;

namespace WebApplication2
{
    public partial class InterFaceTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            var uri = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            var action = "GetNewsList";
            var param = "Action=" + action;
            string content = MD5.GetMd5String(
                string.Format("{0}{1}", param.ToLower(), ConfigurationManager.AppSettings["sec"]));

            var url = string.Format("{0}/Api.ashx?Action={1}&sec={2}", uri, action, content);

            HttpResponse<result> response = new HttpResponse<result>();
            var res = response.GetFuncGetResponse(url, Serilize_Type.Json);

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            string indexDir = Index.INDEX_DIR;
            if (txtName.Text.Trim() != "")
            {
                int recCount = 0;
                List<SeekWebNewsItem> list = Index.Search(indexDir, txtName.Text, 10, 1, out recCount);
                rptList1.DataSource = list;
                rptList1.DataBind();
            }
        }
    }
}