using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.DataAccess.Business
{
    public class ChannelConfigAccess : BaseDataAccess<ChannelConfig>
    {
        public ChannelConfigAccess(string conn)
            : base("ChannelConfig", "ChannelId", conn, "", null, true)
        {
        }
    }
}
