using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.ByEmail
{
    public interface IEmailProcessor
    {
        void SetEmailSettings(EmailSettings emailSettings);
        Task<SmtpClient> SendEmail(string key, string value, string emailSubject);
    }
}
