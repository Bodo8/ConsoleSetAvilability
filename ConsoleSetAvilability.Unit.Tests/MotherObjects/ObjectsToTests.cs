using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Unit.Tests.MotherObjects
{
    internal static class ObjectsToTests
    {
        internal static List<OnlineProduct> GetOnlineProducts()
        {
            return new List<OnlineProduct>()
            {
                new(){ Symbol = "HH-78301_992-43", WebsiteId = 25, Quantity = 0},
                new(){ Symbol = "HH-78301_992-42", WebsiteId = 25, Quantity = 3},
                new(){ Symbol = "HH-78301_992-41", WebsiteId = 25, Quantity = 7},
                new(){ Symbol = "HH-78301_992-40", WebsiteId = 25, Quantity = 15},
                new(){ Symbol = "HH-78301_992-31", WebsiteId = 25, Quantity = 28},
                new(){ Symbol = "HH-78301_992-32", WebsiteId = 25, Quantity = 30},
                new(){ Symbol = "HH-78301_992-33", WebsiteId = 25, Quantity = 40},
            };
        }

        internal static HttpContent CreateParameters(string availability)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "symbol", "symbol" },
                { "availability",  availability}
            };
            string jsonData = JsonConvert.SerializeObject(parameters);

            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }

        internal static List<StatusRangeMath> GetStatusesTwo()
        {
            decimal max = int.MaxValue;
            decimal min = int.MinValue;

            return new List<StatusRangeMath>()
            {
                new StatusRangeMath(){ NameAvailability = "Dostępność do 14 dni", QuantityMax = max, QuantityMin = 6},
                new StatusRangeMath(){ NameAvailability = "Dostępność na zapytanie", QuantityMax = 5, QuantityMin = min}
            };
        }

        internal static ApiProductSettings GetApiProductSettings()
        {
            return new()
            {
                BaseUrl = "https://localhost:5003/", 
                LoginUrl = "api/Account/login",
                UrlWebsites = "api/Websites?WithParameters=true",
                UrlProduct= "api/OnlineProduct/AllIsLatest?IsLatest=true&WebsiteId=",
                UrlNamesRange = "api/Websites/Status/GetNameStatuses",
            };
        }

        internal static List<StatusRangeMath> GetStatusesWithDelay()
        {

            return new List<StatusRangeMath>()
            {
                new StatusRangeMath(){  DaysDelay = 7}
            };
        }

        internal static List<StatusRangeMath> GetStatusesThree()
        {
            decimal max = int.MaxValue;
            decimal min = int.MinValue;

            return new List<StatusRangeMath>()
            {
                new StatusRangeMath(){ NameAvailability = "Dostępność do 48 godzin", QuantityMax = max, QuantityMin = 1},
                new StatusRangeMath(){ NameAvailability = "Brak", QuantityMax = 0, QuantityMin = min},
            };
        }

        internal static List<RangeName> GetNamesRanges()
        {
            return new List<RangeName>() {
                new() { StatusId = 1, Status = "Brak", DelayMaxDays = -1, DelayMinDays = -1, ShopStatusId = 5 },
                new() { StatusId = 2, Status = "Dostępność na zapytanie", DelayMaxDays = 0, DelayMinDays = 0, ShopStatusId = 6 },
                new() { StatusId = 2, Status = "Dostępność do 24 godzin", DelayMaxDays = 1, DelayMinDays = 1, ShopStatusId = 3 },
                new() { StatusId = 2, Status = "Dostępność do 48 godzin", DelayMaxDays = 2, DelayMinDays = 2, ShopStatusId = 11 },
                new() { StatusId = 2, Status = "Dostępność do 14 dni", DelayMaxDays = 14, DelayMinDays = 11, ShopStatusId = 1 },
                new() { StatusId = 2, Status = "Dostępność do 3 dni", DelayMaxDays = 3, DelayMinDays = 3, ShopStatusId = 4 },
                new() { StatusId = 2, Status = "Dostępność do 7 dni", DelayMaxDays = 7, DelayMinDays = 4, ShopStatusId = 2 },
                new() { StatusId = 2, Status = "Dostępność do 10 dni", DelayMaxDays = 10, DelayMinDays = 8, ShopStatusId = 28 },
                new() { StatusId = 2, Status = "Dostępność 7-14 dni", DelayMaxDays = 15, DelayMinDays = 15, ShopStatusId = 27 },  //
                new() { StatusId = 2, Status = "Dostępność do 3 tygodni", DelayMaxDays = 21, DelayMinDays = 16, ShopStatusId = 27 }
            };
        }

        internal static List<StatusRangeMath> GetStatusesMulti()
        {
            decimal max = int.MaxValue;
            decimal min = int.MinValue;

            return new List<StatusRangeMath>()
            {
                new StatusRangeMath(){ NameAvailability = "Brak", QuantityMax = 5, QuantityMin = min},
                new StatusRangeMath(){ NameAvailability = "Dostępność do 3 dni", QuantityMax = 15, QuantityMin = 6},
                new StatusRangeMath(){ NameAvailability = "Dostępność do 7 dni", QuantityMax = 30, QuantityMin = 16},
                new StatusRangeMath(){ NameAvailability = "Dostępność 4-8 tygodni", QuantityMax = max, QuantityMin = 31},
            };
        }

        internal static ApiShopSettings GetApiShopSettings()
        {
            var apiShopSettings = new ApiShopSettings()
            {
                UrlProductDb = "api/Product/",
                BaseUrlShopApi = "https://localhost:5001/",
                UrlOneProduct = "admin/popup.php?k=product&w=&id=",
                UrlProductReserve = "api/ReserveProduct",
                AvailabilityZero = "Dostępność na zapytanie"
            };

            return apiShopSettings;
        }
    }
}
