using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using Model;
using News.DataAccess.Business;
using News.Model;

namespace News.Service.InterFace
{
  
    public class GetNewsCategoryService : NewsInterfaceServcie<NewsCategory>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public SqlQueryParam param;
        protected NewsCategoryAccess access;

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
            param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "CategoryId", true);
            if (request["CategoryName"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "CategoryName", request["CategoryName"]));
            }
            if (request["CategoryId"] != null)
            {
                param.where.where.Add(SqlParamHelper.CreateWhere(
                   PARAM_TYPE.EQUATE, LINK_TYPE.AND, "CategoryId", request["CategoryId"]));
            }
            //CategoryId
            base.InitContent(request);
        }
        public override string GetList()
        {
            myResult = new result();
            int count = 0;
            access = new NewsCategoryAccess(Conn);
            List<NewsCategory> list = access.Load(param, out count);
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
