using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.SqlParam;

namespace News.DataAccess.Business
{
    public class NewsCategoryAccess : BaseDataAccess<NewsCategory>
    {
        public NewsCategoryAccess(string conn)
            : base("NewsCategory", "CategoryId", conn)
        {
        }

        public List<NewsCategory> GetAllCate()
        {
            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "CategoryId", true);
            return Load(param);

        }
    }
}
