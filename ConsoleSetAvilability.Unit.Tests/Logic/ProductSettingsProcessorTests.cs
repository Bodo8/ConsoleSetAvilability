using ConsoleSetAvilability.Library.Logic;
using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using ConsoleSetAvilability.Unit.Tests.MotherObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleSetAvilability.Unit.Tests.Logic
{
    public class ProductSettingsProcessorTests
    {
        private readonly ILogger<IProductSettingsProcessor> _logger;
        private readonly IWebService _webService;
        private readonly AppSettings _appsettings;
        private readonly IProductSettingsProcessor _sut;

        public ProductSettingsProcessorTests()
        {
            var apiShopSettings = ObjectsToTests.GetApiShopSettings();
            var apiProductSettings = ObjectsToTests.GetApiProductSettings();
            _appsettings = new AppSettings() { ApiShopData = apiShopSettings, ApiProductData = apiProductSettings };
            _logger = Substitute.For<ILogger<IProductSettingsProcessor>>();
            _webService = Substitute.For<IWebService>();
            _sut = Substitute.For<IProductSettingsProcessor>();
        }

        [Theory]
        [InlineData(0, 0, "Dostępność na zapytanie")]
        public async Task Should_SetAvailabilitySimpleSettings_ReturnExpect(int settings, int product, string availability)
        {
            List<OnlineProduct> products = GetOnlineProducts();
            var multiList = new List<List<StatusRangeMath>>()
            {
                ObjectsToTests.GetStatusesWithDelay(),
            };

            var clientHttp = new HttpClient();
            var rangeNames = ObjectsToTests.GetNamesRanges();
            var rangeNamesObject = new RangeNamesObject() { RangeNames = rangeNames };
            var url = _appsettings.ApiProductData.BaseUrl + _appsettings.ApiProductData.UrlNamesRange;
            _webService.GetApiContent<RangeNamesObject>(url, clientHttp).Returns(Task.FromResult(rangeNamesObject));
            _webService.GetHttpClient().Returns(clientHttp);

            var sut = new ProductSettingsProcessor(_logger, _webService);

            await sut.SetAvailabilitySimpleSettings(multiList[settings], products[product], _appsettings, clientHttp);

            await _webService.Received(1).SetAvailabilityInShop(availability, products[product], _appsettings.ApiShopData, rangeNames, clientHttp);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 12)]
        [InlineData(2, 0)]
        public void Should_CalculateDayAvailability_ReturnExpect(int index, int expect)
        {
            var dates = new[] { new DateTime(2022, 04, 15), DateTime.MinValue, DateTime.MinValue, new DateTime(2022, 04, 12) };
            var products = new List<OnlineProduct>() { new() { DateDelivery = dates[index], Quantity = 5 },
             new() { DateDelivery = dates[index], Quantity = 5}, new() { DateDelivery = dates[index], Quantity = 0} };
            int current = new DateTime(2022, 04, 12).DayOfYear;
            var statusRange = new StatusRangeMath() { DaysDelay = 7 };
            var sut = new ProductSettingsProcessor(_logger, _webService);

            var actual = sut.CalculateDayAvailability(products[index], statusRange, current);

            actual.Should().NotBe(expect);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 7, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 6, 0)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 4, 2)]
        [InlineData(2, 7, 0)]
        [InlineData(2, 5, 3)]
        public async Task Should_SetAvailabilitySimpleSettings_ReturnExpectArgument(int settings, int product, int availability)
        {
            List<OnlineProduct> products = GetOnlineProducts();
            var multiList = new List<List<StatusRangeMath>>()
            {
                ObjectsToTests.GetStatusesTwo(),
                ObjectsToTests.GetStatusesThree(),
                ObjectsToTests.GetStatusesMulti()
            };

            var clientHttp = new HttpClient();
            var rangeNames = ObjectsToTests.GetNamesRanges();
            var rangeNamesObject = new RangeNamesObject() { RangeNames = rangeNames };
            var url = _appsettings.ApiProductData.BaseUrl + _appsettings.ApiProductData.UrlNamesRange;
            _webService.GetApiContent<RangeNamesObject>(url, clientHttp).Returns(Task.FromResult(rangeNamesObject));
            _webService.GetHttpClient().Returns(clientHttp);

            var sut = new ProductSettingsProcessor(_logger, _webService);

            await sut.SetAvailabilitySimpleSettings(multiList[settings], products[product], _appsettings, clientHttp);

            await _webService.Received(1).SetAvailabilityInShop(multiList[settings][availability].NameAvailability,
                products[product], _appsettings.ApiShopData, rangeNames, clientHttp);
        }

        [Theory]
        [InlineData(0, 7, 1, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(1, 6, 0, 1)]
        public async Task Should_SetAvailabilitySimpleSettings_ReciveOne(int settings, int product, int availability, int availabilityOther)
        {
            List<OnlineProduct> products = GetOnlineProducts();
            var multiList = new List<List<StatusRangeMath>>()
            {
                ObjectsToTests.GetStatusesTwo(),
                ObjectsToTests.GetStatusesThree(),
                ObjectsToTests.GetStatusesMulti()
            };

            var clientHttp = new HttpClient();
            var rangeNames = ObjectsToTests.GetNamesRanges();
            var rangeNamesObject = new RangeNamesObject() { RangeNames = rangeNames };
            var url = _appsettings.ApiProductData.BaseUrl + _appsettings.ApiProductData.UrlNamesRange;
            _webService.GetApiContent<RangeNamesObject>(url, clientHttp).Returns(Task.FromResult(rangeNamesObject));
            _webService.GetHttpClient().Returns(clientHttp);

            var sut = new ProductSettingsProcessor(_logger, _webService);

            await sut.SetAvailabilitySimpleSettings(multiList[settings], products[product], _appsettings, clientHttp);

            await _webService.Received(1).SetAvailabilityInShop(multiList[settings][availability].NameAvailability,
                products[product], _appsettings.ApiShopData, rangeNames, clientHttp);
            await _webService.Received(0).SetAvailabilityInShop(multiList[settings][availabilityOther].NameAvailability, 
                products[product], _appsettings.ApiShopData, rangeNames, clientHttp);
        }

        internal static List<OnlineProduct> GetOnlineProducts()
        {

            return new List<OnlineProduct>()
            {
                new(){ Symbol = "HH-78301_992-43", WebsiteId = 25, Quantity = 0, DateDelivery = DateTime.MinValue },
                new(){ Symbol = "HH-78301_992-42", WebsiteId = 25, Quantity = 3, DateDelivery = DateTime.MinValue},
                new(){ Symbol = "HH-78301_992-41", WebsiteId = 25, Quantity = 7},
                new(){ Symbol = "HH-78301_992-40", WebsiteId = 25, Quantity = 15},  //3
                new(){ Symbol = "HH-78301_992-31", WebsiteId = 25, Quantity = 28},  //4
                new(){ Symbol = "HH-78301_992-32", WebsiteId = 25, Quantity = 31},  //5
                new(){ Symbol = "HH-78301_992-33", WebsiteId = 25, Quantity = 1},   //6
                new(){ Symbol = "HH-78301_992-34", WebsiteId = 25, Quantity = 5},   //7
            };
        }
    }
}
