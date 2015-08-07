using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using Model;
using News.DataAccess.Business;
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
    public class GetNewsListService : NewsInterfaceServcie<NewsItem>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        protected static NewsItemAccess newsItemAccess;
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            newsItemAccess = new NewsItemAccess(Conn);
            List<NewsItem> list = newsItemAccess.Load(param, out count);
            pagenumber = count / pageCount + 1;
            myResult.data = new ResponseData()
            {
                returndata = list,
                pageinfo = new PageInfo()
                {
                    pageindex = pageIndex,
                    pagenumber = pagenumber,
                    pagetotal = count
                }
            };
            myResult.status = new Status()
            {
                code = 500,
                msg = ""
            };
            return SerilizeService<result>.CreateSerilizer(requestType).Serilize(myResult);
        }
        public override void InitContent(HttpRequest request)
        {
            if (request["pageIndex"] != null)
            {
                pageIndex = int.Parse(request["pageIndex"]);
            }
            if (request["pageCount"] != null)
            {
                pageCount = int.Parse(request["pageCount"]);
            }
            param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "NewsId", true);
            if (request["FromSite"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "FromSite", request["FromSite"]));
            }
            if (request["ChannelName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelName", request["ChannelName"]));
            }
            if (request["SourceSite"] != null) //SourceSite
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceSite", request["SourceSite"]));
            }
            base.InitContent(request);
        }
    }
}
