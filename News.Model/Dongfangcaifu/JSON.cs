using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Model.Dongfangcaifu
{
    public class JSON
    {
        public object adlist { get; set; }
        public int AllCount { get; set; }
        public int AtPage { get; set; }
        public long MaxID { get; set; }
        public string me { get; set; }
        public long MinID { get; set; }
        public List<news> news { get; set; }
        public int PageCount { get; set; }
        public int rc { get; set; }

    }
    public class news
    {
        public long id { get; set; }
        public long newsid { get; set; }
        public string url_m { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public DateTime showtime { get; set; }

    }

}
