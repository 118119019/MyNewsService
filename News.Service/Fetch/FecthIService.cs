using News.DataAccess.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Service.Fetch
{
    public interface FecthIService
    {
        void UpdateSiteChl();
        void GetNewsDetail(ChannelConfig chlCfg);
        void Fetch(SiteConfig siteCfg);
    }
}
