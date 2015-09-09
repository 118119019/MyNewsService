using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.DataAccess.Business;

namespace News.Service.MySql
{
    public class SiteConfigMySqlService : DbContext
    {
        public SiteConfigMySqlService()
            : base("DbConMySql")
        { }

        public DbSet<SiteConfig> Blogs { get; set; }
    }
     
}
