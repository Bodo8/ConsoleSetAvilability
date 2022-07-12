using ConsoleSetAvilability.Library.Infrastracture.Interfaces;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.ByEmail
{
    public class EmailNotifications : IEmailNotifications
    {
        private readonly IEmailProcessor _emailProcessor;
        private readonly IEmailComposer _emailComposer;
        private readonly ILogger<IEmailNotifications> _logger;

        public EmailNotifications(IEmailProcessor emailProcessor, IEmailComposer emailComposer, ILogger<IEmailNotifications> logger)
        {
            _emailProcessor = emailProcessor;
            _emailComposer = emailComposer;
            _logger = logger;
        }

        public async Task<List<SmtpClient>> SendEmailNotifications(Dictionary<string, List<ResponseReserveDto>> websitesToUpdate, AppSettings appSettings)
        {
            List<SmtpClient> clients = new List<SmtpClient>();

            if (websitesToUpdate.Count > 0 && websitesToUpdate.Values.First().Count() > 0)
            {
                ConcurrentBag<Dictionary<string, string>> cbag = new ConcurrentBag<Dictionary<string, string>>();
                List<Task> bagAddTasks = new List<Task>();

                string emailSubject = string.Format("Zerowanie: {0}", websitesToUpdate.Keys.First());
                string messageBody = _emailComposer.ComposeTableWithHtml(websitesToUpdate);
                List<string> emailRecipients = appSettings.EmailRecipients;

                foreach (var recipient in emailRecipients)
                {
                    Dictionary<string, string> oneEmail = new Dictionary<string, string>();
                    oneEmail.Add(recipient, messageBody);
                    bagAddTasks.Add(Task.Run(() => cbag.Add(oneEmail)));
                }

                Task.WaitAll(bagAddTasks.ToArray());
                List<Task> bagConsumeTasks = new List<Task>();
                int itemsInBag = 0;

                while (!cbag.IsEmpty)
                {
                    bagConsumeTasks.Add(Task.Run(async () =>
                    {
                        Dictionary<string, string> item;
                        if (cbag.TryTake(out item))
                        {
                            var client = await _emailProcessor.SendEmail(item.First().Key, item.First().Value, emailSubject);
                            clients.Add(client);
                            itemsInBag++;
                        }
                    }));
                }

                Task.WaitAll(bagConsumeTasks.ToArray());
                _logger.LogInformation($"Email z {websitesToUpdate.Keys.First()} wyslany do: {string.Join(", ", emailRecipients)}");

                return clients;
            }

            return clients;
        }

        public void SetEmailSettings(EmailSettings emailSettings)
        {
           _emailProcessor.SetEmailSettings(emailSettings);
        }
    }
}
