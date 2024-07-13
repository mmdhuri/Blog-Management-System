using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Student_Management_System.Interface.Services.EmailServices
{
    public class SEmail
    {
        private readonly SmtpClient _smtpClient;

        public SEmail()
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            _smtpClient = new SmtpClient
            {
                Host = smtpSection.Network.Host,
                Port = smtpSection.Network.Port,
                EnableSsl = smtpSection.Network.EnableSsl,
                Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password)
            };
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("manish.dhuri@hotmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new InvalidOperationException("Could not send email", ex);
            }
        }
    }
}