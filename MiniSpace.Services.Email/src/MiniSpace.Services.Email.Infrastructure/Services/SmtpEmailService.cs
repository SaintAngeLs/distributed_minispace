using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail; 
using System.Net;
using MiniSpace.Services.Email.Application.Services;

namespace MiniSpace.Services.Email.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _password;
        private readonly bool _enableSSL;
        private readonly string _displaySenderEmail;

        public SmtpEmailService(string smtpHost, int smtpPort, string fromEmail, string password, bool enableSSL, string displaySenderEmail)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _fromEmail = fromEmail;
            _password = password;
            _enableSSL = enableSSL;
            _displaySenderEmail = displaySenderEmail;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            string senderEmail = !string.IsNullOrEmpty(_displaySenderEmail) ? _displaySenderEmail : _fromEmail;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "MiniSpace | Email Service"), 
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.EnableSsl = _enableSSL;
                client.Credentials = new NetworkCredential(_fromEmail, _password); 
                await client.SendMailAsync(mailMessage);
            }
        }

    }
}
