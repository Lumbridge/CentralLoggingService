using CLS.Core.StaticData;
using CLS.Sender.Classes;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace CLS.WindowsService.Helpers
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

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                new LogSender("CLS.WindowsService", StaticData.EnvironmentType.DEV, StaticData.SystemType.ConsoleApplication).Log(StaticData.SeverityType.Error, ex);
            }
        }
    }
}
