using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommonService.Serilizer;
using Model;
using News.DataAccess.Business;
using News.Model;
using News.Service.PanGuTool;

namespace News.Service.InterFace
{
    public class SeekNewsListService : NewsInterfaceServcie<SeekWebNewsItem>
    {
        public int pageIndex = 1;
        public int pageCount = 10;
        public int pagenumber = 0;
        public string seekKey = "";
        public override string GetList()
        {
            myResult = new result();

            string indexDir = Index.INDEX_DIR;
            List<SeekWebNewsItem> list = new List<SeekWebNewsItem>();
            int recCount = 0;
            if (seekKey != "")
            {
                list = Index.Search(indexDir, seekKey, pageCount, pageIndex, out recCount);
            }
            pagenumber = recCount / pageCount + 1;
            myResult.data = new ResponseData()
            {
                returndata = list,
                pageinfo = new PageInfo()
                {
                    pageindex = pageIndex,
                    pagenumber = pagenumber,
                    pagetotal = recCount
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
            if (request["SeekKey"] != null)
            {
                seekKey = request["SeekKey"];
            }
            base.InitContent(request);
        }
    }
}
