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
    public class WebShopService : IWebShopService
    {
        private readonly IWebProcessor _webProcessor;
        private readonly ILogger<IWebShopService> _logger;

        public WebShopService(IWebProcessor webProcessor, ILogger<IWebShopService> logger)
        {
            _webProcessor = webProcessor;
            _logger = logger;
        }

        public async Task<ResponseReserveProduct> MakePostWithParameters(
            string url,
            HttpContent? updatedParameters,
            HttpClient shopClient,
            ResponseReserveProduct responseReserve)
        {
            if (updatedParameters != null)
            {
                //string responseDbApi = "{\"isUpdated\": true,\"isSynchronize\": true,\"message\": \"null\"}";  //do testów

                string responseDbApi = await _webProcessor.MakePostRequest(url, updatedParameters, shopClient);

                ResponseReserveProduct? converted = JsonConvert.DeserializeObject<ResponseReserveProduct>(responseDbApi);

                if (converted != null)
                {
                    responseReserve.IsUpdated = converted.IsUpdated;
                    responseReserve.IsSynchronize = converted.IsSynchronize;
                    responseReserve.Message = converted.Message;
                    _logger.LogInformation($"Post Symbol: {responseReserve.Symbol} - IsUpdated: {responseReserve.IsUpdated}," +
                        $" IsSynchronize: {responseReserve.IsSynchronize}, message: {responseReserve.Message}");
                }

                return responseReserve;
            }

            return responseReserve;
        }

        public HttpContent? CreateParameters(OnlineProduct product, string availabilityString, List<RangeName> rangeNames)
        {
            RangeName? range = rangeNames.Where(r => r.Status == availabilityString).FirstOrDefault();

            if (range == null || range.ShopNameStatus == null)
            {
                _logger.LogError($"Brak 'AvailabilityName' zgodnego z shop availability {availabilityString}");
                return null;
            }

            string availability = range.ShopNameStatus;
            string symbol = product.Symbol.Trim();
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "symbol",  symbol},
                { "availability",  availability}
            };
            string jsonData = JsonConvert.SerializeObject(parameters);

            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }
}
