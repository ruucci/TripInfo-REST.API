using System;
using System.Net;
using System.Net.Mail;

namespace TripInfoREST.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];
        private string _mailUsername = Startup.Configuration["mailSettings:azure-username"];
        private string _mailPassword = Startup.Configuration["mailSettings:azure-password"];

        public void Send(string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailFrom),
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(_mailTo);

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential(_mailUsername, _mailPassword),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            smtpClient.Send(mailMessage);
        }
    }
}
