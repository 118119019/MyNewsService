using CommonService.Serilizer;
using Model;
using News.DataAccess.Business;
using News.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using News.Service.MySql;

namespace News.Service.InterFace
{
    public class GetNewsByIdService : NewsInterfaceServcie<NewsItem>
    {
        protected string Id;
        protected static NewsItemMySqlServcie newsItemAccess = new NewsItemMySqlServcie();
        public override string GetList()
        {
            myResult = new result();
            if (Id != "")
            {
                var _id = long.Parse(Id);
                var newsitem = newsItemAccess.Find(p => p.NewsId == _id);
                if (newsitem != null)
                {
                    myResult.data = new ResponseData()
                    {
                        returndata = newsitem,
                        pageinfo = new PageInfo()
                        {
                            pageindex = 1,
                            pagenumber = 1,
                            pagetotal = 1
                        }
                    };
                    myResult.status = new Status()
                    {
                        code = 500,
                        msg = ""
                    };
                }

            }
            return SerilizeService<result>.CreateSerilizer(requestType).Serilize(myResult);
        }
        public override void InitContent(HttpRequest request)
        {
            Id = request["id"] ?? "";
            base.InitContent(request);
        }
    }
}
