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
using System.Linq.Expressions;
using News.Service.MySql;
using System.Data.Common;
using System.Data.SqlClient;

namespace News.Service.InterFace
{
    public class GetNewsListService : NewsInterfaceServcie<NewsItem>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        Expression<Func<NewsItem, bool>> exp;

        protected static NewsItemMySqlServcie newsItemAccess;
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            //newsItemAccess = new NewsItemAccess(Conn);
            newsItemAccess = new NewsItemMySqlServcie();
            Expression<Func<NewsItem, long>> order =
                e => e.NewsId;
            var args = new DbParameter[] {
                   new SqlParameter { ParameterName = "@where", Value = 1} 
                };
            newsItemAccess.Context.Database.ExecuteSqlCommand(
                "", args
                );

            count = newsItemAccess.FindAll(exp).Count;

            List<NewsItem> list = newsItemAccess.GetPage(pageCount, pageCount,
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
            exp = PredicateBuilder.True<NewsItem>();
            param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "NewsId", true);
            if (request["FromSite"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "FromSite", request["FromSite"]));
                exp.And(e => e.FromSite == request["FromSite"]);
            }
            if (request["ChannelName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelName", request["ChannelName"]));
                exp.And(e => e.ChannelName == request["ChannelName"]);
            }
            if (request["SourceSite"] != null) //SourceSite
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SourceSite", request["SourceSite"]));
                int SourceSite = int.Parse(request["SourceSite"]);
                exp.And(e => e.SourceSite == SourceSite);
            }
            if (request["CategoryId"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "CategoryId", request["CategoryId"]));
                var CategoryId = int.Parse(request["CategoryId"]);
                exp.And(e => e.CategoryId == CategoryId);
            }
            base.InitContent(request);
        }
    }
}
