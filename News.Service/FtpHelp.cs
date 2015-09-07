using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonService;

namespace News.Service
{
    public class FtpHelp
    {
        public string config = ConfigurationManager.AppSettings["ftpConfig"];
        public FtpClient ftp;
        public FtpHelp()
        {
            var cfg = config.Split('|');
            ftp = new FtpClient(cfg[0], cfg[1], cfg[2], cfg[3]);
        }

        public void ClearFiles()
        {
            try
            {
                List<string> strList = ftp.ListFiles("");
                foreach (var str in strList)
                {
                    ftp.DeleteFile(str);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException("删除文件出现异常", ex);
            }
        }

        public void Upload(string file)
        {
            ftp.Upload(file, 18000000, 18000000);
        }
    }
}
