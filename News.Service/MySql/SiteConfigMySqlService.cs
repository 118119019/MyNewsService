using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.DataAccess.Business;

namespace News.Service.MySql
{
    public class SiteConfigDbContext : DbContext
    {
        public SiteConfigDbContext()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SiteConfigMap());
            base.OnModelCreating(modelBuilder);
        }        
    }

    public class SiteConfigMySqlService : EFWrapperBase<SiteConfig>
    {
        public SiteConfigMySqlService()
            : base(new SiteConfigDbContext())
        { }

    }
    public class SiteConfigMap : EntityTypeConfiguration<SiteConfig>
    {
        public SiteConfigMap()
        {
            this.ToTable("SiteConfig");
            this.HasKey(p => p.SiteId);
        }
    }

}
