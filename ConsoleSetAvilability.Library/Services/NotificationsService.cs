using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSetAvilability.Library.Infrastracture;
using ConsoleSetAvilability.Library.Infrastracture.ByEmail;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Services.Interfaces;
using ConsoleSetAvilability.Library.Infrastracture.Interfaces;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;

namespace ConsoleSetAvilability.Library.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IEmailNotifications _emailNotifications;
        private readonly IMapper _mapper;
        private readonly ILogger<INotificationsService> _logger;

        public NotificationsService(IEmailNotifications emailNotifications,
            IMapper mapper,
            ILogger<INotificationsService> logger
            )
        {
            _emailNotifications = emailNotifications;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResultNotifications> SendNotifications(List<ResponseReserveProduct> responses, AppSettings settings, string websiteName)
        {
            ResultNotifications result = new ResultNotifications() { EmailIsSended = false };
            List<ResponseReserveProduct> responsesZero = SearchElementsToEmail(responses);
            List<ResponseReserveDto> elementsDto = ConvertToEmailDto(responsesZero);

            var clients = await SendEmails(elementsDto, settings, responses.Count, websiteName);
            result.EmailIsSended = clients.Any();
            _logger.LogWarning($"Uwaga duże zerowanie stanów na 2Click, {websiteName}: {result.EmailIsSended}");

            return result;
        }

        private async Task<List<SmtpClient>> SendEmails(List<ResponseReserveDto> productsDto, AppSettings settings, int numberOfProducts, string websiteName)
        {
            _emailNotifications.SetEmailSettings(settings.EmailParameters);
            string subject = $"Zerowanie stanów na 2Click, {websiteName}";
            Dictionary<string, List<ResponseReserveDto>> websitesToUpdate = CreateDataToNotifications(productsDto, subject, numberOfProducts);
            return await _emailNotifications.SendEmailNotifications(websitesToUpdate, settings);
        }

        public Dictionary<string, List<ResponseReserveDto>> CreateDataToNotifications(List<ResponseReserveDto> productsDto, string websiteName, int numberOfProducts)
        {
            Dictionary<string, List<ResponseReserveDto>> result = new Dictionary<string, List<ResponseReserveDto>>();

            if (productsDto.Count() > 0)
            {
                var emailSubject = $"{websiteName} sprawdzono {numberOfProducts} zmiana w {productsDto.Count}";
                result.Add(emailSubject, productsDto);
            }

            return result;
        }

        private List<ResponseReserveProduct> SearchElementsToEmail(List<ResponseReserveProduct> responses)
        {
            return responses.Where(p => p.WasSetsToZero).ToList();
        }

        private List<ResponseReserveDto> ConvertToEmailDto(List<ResponseReserveProduct> elementsToEmail)
        {
            return _mapper.Map<List<ResponseReserveDto>>(elementsToEmail);
        }
    }
}
