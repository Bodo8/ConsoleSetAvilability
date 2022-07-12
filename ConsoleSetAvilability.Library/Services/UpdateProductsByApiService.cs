using ConsoleSetAvilability.Library.Commons.Exceptions;
using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Services
{
    public class UpdateProductsByApiService : IUpdateProductsByApiService
    {
        private readonly ILogger<IUpdateProductsByApiService> _logger;
        private readonly IProductSettingsProcessor _productSettingsProcessor;
        private readonly INotificationsService _notificationsService;

        public UpdateProductsByApiService(
            ILogger<IUpdateProductsByApiService> logger,
            IProductSettingsProcessor productSettingsProcessor,
            INotificationsService notificationsService)
        {
            _logger = logger;
            _productSettingsProcessor = productSettingsProcessor;
            _notificationsService = notificationsService;
        }

        public async Task<List<ResponseReserveProduct>> UpdateProducts(
            OnlineProductsObject? productsObject,
            Website website,
            AppSettings appSettings,
            HttpClient apiClient)
        {
            List<OnlineProduct> productsToProcess = productsObject == null ? new List<OnlineProduct>() : productsObject.Products;
            List<ResponseReserveProduct> responsesReserve = new List<ResponseReserveProduct>();
            List<ResponseReserveProduct> zeroResponses = new List<ResponseReserveProduct>();
            List<StatusRangeMath> statuses = ConvertNullMinMax(website.StatusRanges);

            foreach (var product in productsToProcess)
            {
                var response = await _productSettingsProcessor.SetAvailabilitySimpleSettings(statuses, product, appSettings, apiClient);

                SelectResponses(responsesReserve, zeroResponses, response);
            }

            bool sendEmail = CheckHowMuchSetToZero(productsToProcess.Count(), zeroResponses, appSettings.PercentSetToZero);

            if (sendEmail)
            {
                ResultNotifications response = await _notificationsService.SendNotifications(zeroResponses, appSettings, website.WebsiteName);
                _logger.LogWarning($"Wysłano email z alertem o dużej ilości zerowań {response.EmailIsSended}");
            }

            return responsesReserve;
        }

        private void SelectResponses(List<ResponseReserveProduct> responsesReserve, List<ResponseReserveProduct> zeroResponses, ResponseReserveProduct response)
        {
            if ((response.IsUpdated == false || response.IsSynchronize == false)
            && (response.Message != null && !response.Message.Contains("Już było")))
                responsesReserve.Add(response);

            if (response.IsUpdated && response.WasSetsToZero)
                zeroResponses.Add(response);
        }

        public List<StatusRangeMath> ConvertNullMinMax(List<StatusRange> statusRanges)
        {
            List<StatusRangeMath> result = new List<StatusRangeMath>();

            statusRanges.ForEach(s =>
           {
               decimal max = 0;
               decimal min = 0;
               GetMinMaxDecimalValue(s, out max, out min);

                   var status = new StatusRangeMath()
                   {
                       QuantityMin = min,
                       QuantityMax = max,
                       NameAvailability = s.NameAvailability,
                       DaysDelay = s.DaysDelay,
                   };
                   result.Add(status);
               

           });

            return result;
        }

        private void GetMinMaxDecimalValue(StatusRange s, out decimal max, out decimal min)
        {
            decimal minValue = int.MinValue;
            decimal maxValue = int.MaxValue;

            if (s.QuantityMin == null)
                min = minValue;
            else 
                decimal.TryParse(s.QuantityMin.ToString(), out min);
                 
            if (s.QuantityMax == null)
                max = maxValue;
            else
                decimal.TryParse(s.QuantityMax.ToString(), out max);

            if (s.QuantityMax == null && s.QuantityMin == null)
            {
                min = minValue;
                max = 0;
            }
        }

        public bool CheckHowMuchSetToZero(int productsQuantity, List<ResponseReserveProduct> responsesReserve, int percentSetToZero)
        {
            decimal zeros = 0;
            decimal reponsesZero = 0;

            if (responsesReserve.Count > 0)
                reponsesZero = responsesReserve.Where(p => p?.WasSetsToZero == true).Count();

            decimal quantityProducts = decimal.Parse(productsQuantity.ToString());
            
            if (productsQuantity > 0)
                zeros = Math.Round((reponsesZero / quantityProducts) * 100, 2);

            if(zeros >= percentSetToZero)
                return true;
            else 
                return false;
        }
    }
}
