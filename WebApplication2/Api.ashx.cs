using CommonService.Serilizer;
using DataAccess;
using DataAccess.SqlParam;
using News.DataAccess.Business;
using News.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    /// <summary>
    /// Api 的摘要说明
    /// </summary>
    public class Api : IHttpHandler
    {
        protected static string Conn = ConfigurationManager.ConnectionStrings["LinstenNews"].ConnectionString;
        protected static NewsItemAccess newsItemAccess = new NewsItemAccess(Conn);
        protected static ChannelConfigAccess chlCfgAccess = new ChannelConfigAccess(Conn);
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var action = request["Action"] ?? "";
            response.ContentType = "application/json";
            if (action == "")
            {

                context.Response.Write("Hello World");
            }
            else
            {
                //方法 getlist getById
                //int pageIndex, int pageCount, string orderField, bool isDesc
                int pageIndex = 1;
                int pageCount = 10;
                int pagenumber = 0;
                string outString = "";
                if (request["pageIndex"] != null)
                {
                    pageIndex = int.Parse(request["pageIndex"]);
                }
                if (request["pageCount"] != null)
                {
                    pageCount = int.Parse(request["pageCount"]);
                }

                var myResult = new result();
                var param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "NewsId", true);
                int count = 0;
                switch (action)
                {
                    case "GetNewsList":
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

                        List<NewsItem> list = newsItemAccess.Load(param, out count);
                        pagenumber = count / pageCount + 1;
                        myResult.data = new ResponseData()
                        {
                            returndata = list,
                            pageinfo = new Model.PageInfo()
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
                        break;
                    case "GetNewsById":
                        if (request["id"] != null)
                        {
                            var newsitem = newsItemAccess.GetContentByPk(request["id"]);
                            if (newsitem != null)
                            {
                                myResult.data = new ResponseData()
                                {
                                    returndata = newsitem,
                                    pageinfo = new Model.PageInfo()
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
                        break;
                    case "GetChannelConfigList":
                        param = SqlParamHelper.GetDefaultParam(pageIndex, pageCount, "ChannelId", true);
                        if (request["ChannelName"] != null)
                        {
                            param.where.where.Add(SqlParamHelper.CreateWhere(
                               PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelName", request["ChannelName"]));
                        }

                        List<ChannelConfig> chlCfglist = chlCfgAccess.Load(param, out count);
                        pagenumber = count / pageCount + 1;
                        myResult.data = new ResponseData()
                        {
                            returndata = chlCfglist,
                            pageinfo = new Model.PageInfo()
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
                        break;
                    default:
                        break;
                }
                outString = SerilizeService<result>.CreateSerilizer(Serilize_Type.Json).Serilize(myResult);
                response.Write(outString);

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