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
    public class NewsItemDbContext : DbContext
    {
        public NewsItemDbContext()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           modelBuilder.Configurations.Add(new NewsItemMap());
           base.OnModelCreating(modelBuilder);
        }
    }
    public class NewsItemMap : EntityTypeConfiguration<NewsItem>
    {
        public NewsItemMap()
        {
            this.ToTable("NewsItem");
            this.HasKey(p => p.NewsId);
        }
    }
    public class WebNewsItemDbContext : DbContext
    {
        public WebNewsItemDbContext()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new WebNewsItemMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class WebNewsItemMap : EntityTypeConfiguration<WebNewsItem>
    {
        public WebNewsItemMap()
        {
            this.ToTable("NewsItem");
            this.HasKey(p => p.NewsId);
        }
    }
    public class NewsItemMySqlServcie : EFWrapperBase<NewsItem>
    {
        public NewsItemMySqlServcie()
            : base(new NewsItemDbContext())
        { }
    }
    public class WebNewsItemMySqlServcie : EFWrapperBase<WebNewsItem>
    {
        public WebNewsItemMySqlServcie()
            : base(new WebNewsItemDbContext())
        { }
    }
}
