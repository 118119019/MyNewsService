using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.MySql
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (BloggingContext db = new BloggingContext())
                {
                    db.Blog.Add(new Blog { Name = "Another Blog" });
                    db.SaveChanges();

                    foreach (var blog in db.Blog)
                    {
                        Response.Write(blog.BlogId + " "
                            + blog.Name + " ");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }

    public class BloggingContext : DbContext
    {

        public BloggingContext()
            : base("DbConMySql")
        { }

        public DbSet<Blog> Blog { get; set; }


    }
    public class Blog
    {
        public int BlogId { get; set; }

        public string Name { get; set; }


    }

}