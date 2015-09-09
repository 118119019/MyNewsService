using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class SiteConfig
    {
        [Key]
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string IndexUrl { get; set; }
        public int Status { get; set; }
        public string ChannelUrl { get; set; }

        public string DetailUrl { get; set; }
    }
}
