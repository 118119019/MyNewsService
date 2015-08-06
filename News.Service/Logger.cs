using net91com.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Service
{
    public static class Logger
    {
        public static void WriteInfo(string info)
        {
#if Dev
            Console.WriteLine(string.Format("{0} {1}", DateTime.Now, info));
#endif
            LogHelper.WriteInfo(info);
        }

        public static void WriteException(string info, Exception ex)
        {
#if Dev
            Console.WriteLine(string.Format("{0} {1} {2}", DateTime.Now, info, ex));
#endif
            LogHelper.WriteException(info, ex);
        }
    }
}
