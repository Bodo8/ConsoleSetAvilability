using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.ByEmail
{
    public class EmailProcessor : IEmailProcessor
    {

        public EmailSettings _emailSettings { get; set; }

        public void SetEmailSettings(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task<SmtpClient> SendEmail(string recipient, string messageBody, string emailSubject)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Synchronizator", _emailSettings.EmailAddress));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = emailSubject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = messageBody;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.SmtpServer, 587, false);
                client.Authenticate(_emailSettings.EmailAddress, _emailSettings.EmailPassword);

                await client.SendAsync(message);

                return client;
            }
        }
    }
}
