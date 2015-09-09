using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace News.DataAccess.Business
{
    public class ChannelConfig
    {
        [Key]
        public long ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelVal { get; set; }
        public int SiteId { get; set; }
    }
}
