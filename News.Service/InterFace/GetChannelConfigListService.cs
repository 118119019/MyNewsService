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
using System.Linq.Expressions;
using News.Service.MySql;

namespace News.Service.InterFace
{
    public class GetChannelConfigListService : NewsInterfaceServcie<ChannelConfig>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        Expression<Func<ChannelConfig, bool>> exp;
        protected ChannelConfigMySqlService access;

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
            exp = PredicateBuilder.True<ChannelConfig>();
            if (request["ChannelName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelName", request["ChannelName"]));
                exp.And(p => p.ChannelName == request["ChannelName"]);
            }
            base.InitContent(request);
        }
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            access = new ChannelConfigMySqlService();
            count = access.GetCount(exp);
            Expression<Func<ChannelConfig, long>> order =
                e => e.ChannelId;
            List<ChannelConfig> list = access.GetPage(pageIndex, pageCount,
                          order, false, exp);
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
