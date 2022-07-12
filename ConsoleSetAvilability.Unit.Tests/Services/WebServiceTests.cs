using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Services;
using ConsoleSetAvilability.Library.Services.Interfaces;
using ConsoleSetAvilability.Unit.Tests.MotherObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleSetAvilability.Unit.Tests.Services
{
    public class WebServiceTests
    {
        private readonly ILogger<IWebService> _logger;
        private readonly IWebProcessor _webProcessor;
        private readonly IWebShopService _shopService;

        public WebServiceTests()
        {
            _logger = Substitute.For<ILogger<IWebService>>();
            _webProcessor = Substitute.For<IWebProcessor>();
            _shopService = Substitute.For<IWebShopService>();
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(2, 2, true)]
        [InlineData(1, 1, false)]
        [InlineData(2, 3, false)]
        public void Should_CheckDoNeedSetNewAvailability_ReturnExpect(int settings, int product, bool expect)
        {
            var sut = new WebService(_webProcessor, _logger, _shopService);
            var shopProducts = new[] {
            new ShopProduct() { Visible = false, Unavailable = true, AvailabilityId = 6 },
            new ShopProduct() { Visible = true, AvailabilityId = 6 },
            new ShopProduct() { Visible = true, AvailabilityId = 6 },
            new ShopProduct() { Visible = false, Unavailable = false, AvailabilityId = 6 },
            };
            var names = new[] { "Dostępność na zapytanie", "Dostępność na zapytanie", "Dostępność do 7 dni" };
            var ranges = ObjectsToTests.GetNamesRanges();


            var actual = sut.CheckDoNeedSetNewAvailability(shopProducts[product], names[settings], ranges);

            actual.Should().Be(expect);
        }

        [Fact]
        public void Test()
        {
            string symbol = "1553523117010";
            symbol = symbol.Insert(5, "-");
            symbol = symbol.Insert(9, "-");

        }
    }
}
