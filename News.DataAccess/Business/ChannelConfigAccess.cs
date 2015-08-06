using DataAccess;
using DataAccess.SqlParam;
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

        public void SaveChlCfg(ChannelConfig cfg)
        {
            var param = SqlParamHelper.GetDefaultParam(1, int.MaxValue, "SiteId", true);
            param.where.where.Add(SqlParamHelper.CreateWhere(
                    PARAM_TYPE.EQUATE, LINK_TYPE.AND, "SiteId", cfg.SiteId.ToString()));
            param.where.where.Add(SqlParamHelper.CreateWhere(
                    PARAM_TYPE.EQUATE, LINK_TYPE.AND, "ChannelVal", cfg.ChannelVal));
            var chlCfg = Load(param).FirstOrDefault();
            if (chlCfg == null)
            {
                chlCfg = new ChannelConfig();
                chlCfg.ChannelId = -1;
            }
            chlCfg.ChannelName = cfg.ChannelName;
            chlCfg.ChannelVal = cfg.ChannelVal;
            chlCfg.SiteId = cfg.SiteId;
            Save(chlCfg, chlCfg.ChannelId.ToString());
        }
    }
}
