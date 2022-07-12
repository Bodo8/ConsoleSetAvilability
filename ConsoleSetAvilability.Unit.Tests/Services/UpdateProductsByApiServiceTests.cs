using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services;
using ConsoleSetAvilability.Library.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConsoleSetAvilability.Unit.Tests.Services
{
    public class UpdateProductsByApiServiceTests
    {
        private readonly ILogger<IUpdateProductsByApiService> _logger;
        private readonly IProductSettingsProcessor _productSettingsProcessor;
        private readonly INotificationsService _notificationsService;
        private readonly UpdateProductsByApiService _sut;

        public UpdateProductsByApiServiceTests()
        {
            _logger = Substitute.For<ILogger<IUpdateProductsByApiService>>();
            _productSettingsProcessor = Substitute.For<IProductSettingsProcessor>();
            _notificationsService = Substitute.For<INotificationsService>();
            _sut = new UpdateProductsByApiService(_logger, _productSettingsProcessor, _notificationsService);
        }

        [Theory]
        [InlineData(70, 1, true)]
        [InlineData(90, 1, false)]
        [InlineData(90, 0, false)]
        public void Should_CheckHowMuchSetToZero_ReturnExpect(int percent, int size, bool expect)
        {
            List<ResponseReserveProduct> responsesReserve = new();

            if (size > 0)
            {
                responsesReserve.Add(new ResponseReserveProduct() { WasSetsToZero = true });
                responsesReserve.Add(new ResponseReserveProduct() { WasSetsToZero = true });
                responsesReserve.Add(new ResponseReserveProduct() { WasSetsToZero = true });
                responsesReserve.Add(new ResponseReserveProduct() { WasSetsToZero = true });
                responsesReserve.Add(new ResponseReserveProduct() { WasSetsToZero = false });
            }

            var actual = _sut.CheckHowMuchSetToZero(responsesReserve.Count(), responsesReserve, percent);

            actual.Should().Be(expect);
        }

        [Fact]
        public void Should_ConvertNullMinMax_ReturnDigits()
        {
            var statuses = new List<StatusRange>() {
             new StatusRange() { QuantityMax = 5, QuantityMin = null , NameAvailability = "Brak"},
             new StatusRange() { QuantityMax = null, QuantityMin = 1, NameAvailability = "48h" },
             new StatusRange() { QuantityMax = 5, QuantityMin = 1, NameAvailability = "24h" },
             new StatusRange() { QuantityMax = null, QuantityMin = null, NameAvailability = "Brak" }
            };
            decimal max = int.MaxValue;
            decimal min = int.MinValue; 

            var actual = _sut.ConvertNullMinMax(statuses);

            actual[0].QuantityMax.Should().Be(5);
            actual[0].QuantityMin.Should().Be(min);
            actual[1].QuantityMax.Should().Be(max);
            actual[1].QuantityMin.Should().Be(1);
            actual[2].QuantityMax.Should().Be(5);
            actual[2].QuantityMin.Should().Be(1);
            actual[3].QuantityMax.Should().Be(0);
            actual[3].QuantityMin.Should().Be(min);
        }
    }
}
