using CommonService;
using News.Service.InterFace;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2
{
    public class RequestHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string strDomain = context.Request["Action"] ?? "";
            if (UrlAuthorize(context))
            {
                strDomain = strDomain.ToLower();
                DealRequest(context, strDomain);
            }
        }
        /// <summary>
        /// 加密验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool UrlAuthorize(HttpContext context)
        {
            string[] str = context.Request.Url.AbsoluteUri.Split('?');
            if (str.Length < 2)
            {
                //Message = "参数错误";
                return false;
            }
            string struri = str[1].ToLower();
            string sec = context.Request["sec"];
            struri = struri.Replace(string.Format("&sec={0}", sec.ToLower()), "");
            string secsigned = GetAuthorizeSec(struri);
            if (sec != secsigned)
            {
                //Message = "加密结果错误！";
                return false;
            }
            return true;
        }
        private string GetAuthorizeSec(string param)
        {
            string content = string.Format("{0}{1}", param, ConfigurationManager.AppSettings["sec"]);
            return MD5.GetMd5String(content);
        }
        private void DealRequest(HttpContext context, string strDomain)
        {
            IService service = CreateService(context, strDomain);
            if (null == service)
            {
                context.Response.StatusCode = 404;
                context.Response.Output.Write("请求方法不存在");
                return;
            }
            string strcontent;
            if (!service.CheckValid(context.Request, out strcontent))
            {
                context.Response.StatusCode = 403;
                context.Response.Output.Write(strcontent == "" ? "没有授权！" : strcontent);
                return;
            }
            string strout = null == context.Request["output"] ? "json" : context.Request["output"].ToLower();
            context.Response.ContentType = "json" == strout ? "text/json" : "text/xml";
            service.InitContent(context.Request);
            string str;
            if (context.Request.ServerVariables["Request_Method"] == "POST")
            {
                str = service.DealPost();
            }
            else
            {
                str = service.DealGet();
            }
            context.Response.Output.Write(str);
        }

        protected virtual IService CreateService(HttpContext context, string strDomain)
        {
            switch (strDomain)
            {
                case "getnewslist":
                    return new GetNewsListService();
                case "getnewsbyid":
                    return new GetNewsByIdService();
                case "getchannelconfiglist":
                    return new GetChannelConfigListService();
                default:
                    return null;

            }
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