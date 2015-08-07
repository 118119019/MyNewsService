using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using Model;
using News.DataAccess.Business;
using News.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace News.Service.InterFace
{
    public class GetChannelConfigListService : NewsInterfaceServcie<ChannelConfig>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        protected ChannelConfigAccess access;
        
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
            param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "ChannelId", true);
            if (request["ChannelName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelName", request["ChannelName"]));
            }            
            base.InitContent(request);
        }
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            access = new ChannelConfigAccess(Conn);
            List<ChannelConfig> list = access.Load(param, out count);
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
    }
}
