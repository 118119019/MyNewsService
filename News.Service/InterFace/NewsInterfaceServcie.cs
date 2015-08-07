using CommonService.Serilizer;
using DataAccess;
using News.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace News.Service.InterFace
{
    public abstract class NewsInterfaceServcie<T> : IService
    {
        protected Serilize_Type requestType;
        protected result myResult;      
        public string Conn;

        public virtual bool CheckValid(HttpRequest request, out string str)
        {
            str = "";
            return true;
        }

        public string DealGet()
        {
            return GetList();
        }

        public virtual string GetList()
        {
            return "";
        }

        public string DealPost()
        {
            return GetList();
        }

        public virtual void InitContent(HttpRequest request)
        {
            Conn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
            if (null == request["output"])
            {
                requestType = Serilize_Type.Json;
                return;
            }
            requestType = "json" == request["output"].ToLower() ? Serilize_Type.Json : Serilize_Type.Xml;
        }
    }
}
