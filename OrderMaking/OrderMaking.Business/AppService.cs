using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace OrderMaking.Business
{
    public class AppService
    {
        public void SendMail(IList<string> files, string subject)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("nishalocalconvenientstore@gmail.com", "Nisha Local Convenience Store"),
                Subject = subject,
                Body = "<h1>Hi, <br> Find the attached shopping list</h1>",
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress("ravi.chandran69@outlook.com"));

            var toAddressesStr = ConfigurationManager.AppSettings["ToAddresses"];
            if (!string.IsNullOrEmpty(toAddressesStr))
            {
                var toAddresses = toAddressesStr.Split(';');
                foreach (var toAddress in toAddresses)
                {
                    if(!mail.To.Contains( new MailAddress(toAddress)))
                        mail.To.Add(toAddress);
                }
            }

            foreach (var file in files)
            {
                mail.Attachments.Add(new Attachment(file));
            }

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("nishalocalconvenientstore@gmail.com", "thisismystore");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }
}
