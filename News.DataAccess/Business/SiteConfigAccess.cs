using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class SiteConfigAccess: BaseDataAccess<SiteConfig>
    {
        public SiteConfigAccess(string conn)
            : base("SiteConfig", "SiteId", conn)
        {
        }
    }
}
