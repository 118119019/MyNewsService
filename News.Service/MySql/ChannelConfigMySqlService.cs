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
    public class ChannelConfigDbContext : DbContext
    {
        public ChannelConfigDbContext()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChannelConfigMap());
            base.OnModelCreating(modelBuilder);
        }
    }    
    public class ChannelConfigMap : EntityTypeConfiguration<ChannelConfig>
    {
        public ChannelConfigMap()
        {
            this.ToTable("ChannelConfig");
            this.HasKey(p => p.ChannelId);
        }
    }
    public class ChannelConfigMySqlService : EFWrapperBase<ChannelConfig>
    {
        public ChannelConfigMySqlService()
            : base(new ChannelConfigDbContext())
        { }
    }
}
