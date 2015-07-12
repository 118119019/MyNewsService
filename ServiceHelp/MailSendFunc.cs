using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelp
{
    public class MailSendFunc
    {
        public void SendMail(string mail, string content, string title, ResetPsdMailItem emailConfig, bool isHtml = false)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(mail);
                msg.From = new MailAddress(emailConfig.Email, "", Encoding.UTF8);
                msg.Subject = title;//邮件标题   
                msg.SubjectEncoding = Encoding.UTF8;//邮件标题编码   
                msg.Body = content;//邮件内容   
                msg.BodyEncoding = Encoding.UTF8;//邮件内容编码   
                msg.IsBodyHtml = isHtml;//是否是HTML邮件   
                msg.Priority = MailPriority.Normal;//邮件优先级   
                SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential(emailConfig.Email,
                   emailConfig.Psd);
                client.Host = "smtp." + emailConfig.Email.Split('@')[1];
                client.EnableSsl = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);

            }
            catch (Exception ex)
            {
              
            }
        }
    }



    public class ResetPsdMailItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }
        public string Psd { get; set; }
    }
    public class ResetPsdMailConfig
    {
        public List<ResetPsdMailItem> ListItem { get; set; }
    }
}
