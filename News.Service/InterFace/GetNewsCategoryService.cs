using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using Model;
using News.DataAccess.Business;
using News.Model;
using News.Service.MySql;

namespace News.Service.InterFace
{

    public class GetNewsCategoryService : NewsInterfaceServcie<NewsCategory>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        protected NewsCategoryMySqlServcie access;
        Expression<Func<NewsCategory, bool>> exp;
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
            exp = PredicateBuilder.True<NewsCategory>();
            param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "CategoryId", true);
            if (request["CategoryName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "CategoryName", request["CategoryName"]));
                exp.And(p => p.CategoryName == request["CategoryName"]);
            }
            if (request["CategoryId"] != null)
            {
                var id = int.Parse(request["CategoryId"]);
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "CategoryId", request["CategoryId"]));
                exp.And(p => p.CategoryId == id);
            }
            //CategoryId
            base.InitContent(request);
        }
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            access = new NewsCategoryMySqlServcie();
            Expression<Func<NewsCategory, long>> order =
                e => e.CategoryId ;
            count = access.GetCount(exp);
            List<NewsCategory> list = access.GetPage(pageIndex, pageCount,
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
