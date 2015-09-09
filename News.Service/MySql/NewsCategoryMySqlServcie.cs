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
    public class NewsCategoryMySqlServcie : DbContext
    {
        public NewsCategoryMySqlServcie()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NewsCategoryMap());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<NewsCategory> NewsCategory { get; set; }
    }
    public class NewsCategoryMap : EntityTypeConfiguration<NewsCategory>
    {
        public NewsCategoryMap()
        {
            this.ToTable("NewsCategory");
            this.HasKey(p => p.CategoryId );
            this.Property(p => p.CategoryName).IsRequired();
        }
    }

     
}
