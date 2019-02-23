using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Common.Helper
{
    public static class MailHelper
    {
        public static void SendEmail(string fromAddress, string receiveAddress, string ccAddress, string subject, string content, string attachments)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);

            foreach (string everyReceiveAddress in receiveAddress.Split(','))
            {
                mail.To.Add(everyReceiveAddress);
            }

            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;

            foreach (string everyCcAddress in ccAddress.Split(','))
            {
                mail.CC.Add(everyCcAddress);
            }

            foreach (string everyAttachment in attachments.Split(','))
            {
                if(File.Exists(everyAttachment))
                    mail.Attachments.Add(new Attachment(everyAttachment));
            }

            SmtpClient smtp = new SmtpClient("smtp.taifex.com.tw");

            // 看起來不用帳號密碼也能寄出
            //smtp.Credentials = new NetworkCredential("taifexsocial", "Taifex201511");

            try
            {
                smtp.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
