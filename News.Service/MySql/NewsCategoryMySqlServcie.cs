using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using News.DataAccess.Business;
using News.Service.MySql;

namespace News.Service.MySql
{
    public class NewsCategoryDbContext : DbContext
    {
        public NewsCategoryDbContext()
            : base("DbConMySql")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NewsCategoryMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class NewsCategoryMySqlServcie : EFWrapperBase<NewsCategory>
    {
        public NewsCategoryMySqlServcie()
            : base(new NewsCategoryDbContext())
        { }

        /// <summary>
        /// 通过反射一次性将表进行映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    var typesRegister = Assembly.GetExecutingAssembly().GetTypes()
        //        .Where(type => !(string.IsNullOrEmpty(type.Namespace))).Where(type => type.BaseType != null
        //        && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
        //    foreach (var type in typesRegister)
        //    {
        //        dynamic configurationInstance = Activator.CreateInstance(type);
        //        modelBuilder.Configurations.Add(configurationInstance);
        //    }

        //}


    }
    public class NewsCategoryMap : EntityTypeConfiguration<NewsCategory>
    {
        public NewsCategoryMap()
        {
            this.ToTable("NewsCategory");
            this.HasKey(p => p.CategoryId);
            this.Property(p => p.CategoryName).IsRequired();
        }
    }


}
