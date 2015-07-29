using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class ChannelConfig
    {
        public long ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelVal { get; set; }
        public int SiteId { get; set; }
    }
}
