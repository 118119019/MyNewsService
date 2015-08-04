using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Model
{
    public class result
    {
        public ResponseData data { get; set; }
        public Status status { get; set; }
    }
    public class ResponseData
    {
        public Object returndata { get; set; }
        public PageInfo pageinfo { get; set; }
    }
    public class Status
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

}
