using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class NewsCategoryAccess : BaseDataAccess<NewsCategory>
    {
        public NewsCategoryAccess(string conn)
            : base("NewsCategory", "CategoryId", conn)
        {
        }
    }
}
