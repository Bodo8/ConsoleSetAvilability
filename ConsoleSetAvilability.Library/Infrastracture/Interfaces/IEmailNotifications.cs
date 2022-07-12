using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleSetAvilability.Library.Infrastracture.Interfaces
{
    public interface IEmailNotifications
    {
        Task<List<SmtpClient>> SendEmailNotifications(
           Dictionary<string, List<ResponseReserveDto>> websitesToUpdate,
           AppSettings appSettings);

        void SetEmailSettings(EmailSettings emailSettings);
    }
}
