using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    /// <summary>
    /// Api 的摘要说明
    /// </summary>
    public class Api : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var action = request["action"] ?? "";
            if (action == "")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Hello World");
            }
            else
            {
                //方法 getList getById
            }


            //http://msdn.microsoft.com/en-us/library/system.web.httpresponse.binarywrite(v=vs.110).aspx

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