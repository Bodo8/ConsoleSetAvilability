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

namespace ConsoleSetAvilability.Library.Logic
{
    public class ProductSettingsProcessor : IProductSettingsProcessor
    {
        private readonly ILogger<IProductSettingsProcessor> _logger;
        private readonly IWebService _webService;
        private readonly HttpClient _shopClient;

        public ProductSettingsProcessor(ILogger<IProductSettingsProcessor> logger, IWebService webService)
        {
            _logger = logger;
            _webService = webService;
            _shopClient = webService.GetHttpClient();
        }

        public async Task<ResponseReserveProduct> SetAvailabilitySimpleSettings(
            List<StatusRangeMath> websiteSettings,
            OnlineProduct product,
            AppSettings appSettings,
            HttpClient apiClient)
        {
            ResponseReserveProduct response = new ResponseReserveProduct();
            decimal productQuantity = product.Quantity;
            string url = appSettings.ApiProductData.BaseUrl + appSettings.ApiProductData.UrlNamesRange;
            RangeNamesObject ? rangeNamesObject = await _webService.GetApiContent<RangeNamesObject>(url, apiClient);
            List<RangeName> rangeNames = rangeNamesObject.RangeNames;

            foreach (var settings in websiteSettings)
            {
                decimal quantityMax = settings.QuantityMax;
                decimal quantityMin = settings.QuantityMin;
                string nameAvailability = "";
                bool matched = false;

                if (settings.NameAvailability == null)
                {
                    int dayAvailability = CalculateDayAvailability(product, settings, DateTime.Now.DayOfYear);

                    foreach (RangeName rangeData in rangeNames)
                    {
                        if (dayAvailability >= rangeData.DelayMinDays && dayAvailability <= rangeData.DelayMaxDays)
                        {
                            nameAvailability = rangeData.Status;
                            response = await _webService.SetAvailabilityInShop(nameAvailability, product, appSettings.ApiShopData, rangeNames, _shopClient);
                            matched = true;
                        }
                    }
                }
                else
                {
                    nameAvailability = settings.NameAvailability;

                    if (productQuantity >= quantityMin && productQuantity <= quantityMax)
                    {
                        response = await _webService.SetAvailabilityInShop(nameAvailability, product, appSettings.ApiShopData, rangeNames, _shopClient);
                        matched = true;
                    }
                }

                if (matched)
                    _logger.LogInformation($"Response: {nameAvailability}," +
                            $" isUpdate: {response?.IsUpdated}, symbol:{ response?.Symbol}, zerowanie: { response?.WasSetsToZero}," +
                            $" stan z db: {productQuantity}: max: {quantityMax}, min;  {quantityMin} ");
            }

            return response;
        }

        public int CalculateDayAvailability(OnlineProduct product, StatusRangeMath settings, int currentDayNumber)
        {
            if (product.DateDelivery == DateTime.MinValue)
            {
                if (product.Quantity < 1)
                    return 0;
                else
                {
                    int year = DateTime.Now.Year; 
                    DateTime currentDate = new DateTime(year, 1, 1).AddDays(currentDayNumber - 1);
                    var currentDayWeek = currentDate.DayOfWeek;
                    int endWeek = 7 - (int)currentDayWeek;
                    product.DateDelivery = DateTime.Now.AddDays(endWeek);
                }
            }

            int productDelivery = product.DateDelivery.DayOfYear;
            int daysDelay = 0;

            if (settings.DaysDelay != null)
                daysDelay = (int)settings.DaysDelay;

            return (productDelivery - currentDayNumber) + daysDelay;
        }
    }
}
