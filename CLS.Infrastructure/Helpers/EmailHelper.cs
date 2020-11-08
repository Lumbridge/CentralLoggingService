using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace CLS.Infrastructure.Helpers
{
    public static class EmailHelper
    {
        public static void SendEmail(string recipient, string subject, string emailBody)
        {
            var mail = new MailMessage(
                ConfigurationManager.AppSettings["EmailSenderUsername"],
                recipient,
                subject,
                emailBody);

            var client = new SmtpClient
            {
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = ConfigurationManager.AppSettings["SmtpServerEndpoint"],
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["EmailSenderUsername"],
                    ConfigurationManager.AppSettings["EmailSenderPassword"])
            };

            client.Send(mail);
        }
    }
}
