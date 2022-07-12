using ConsoleSetAvilability.Library.Infrastracture;
using ConsoleSetAvilability.Library.Infrastracture.ByEmail;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleSetAvilability.Unit.Tests.Infrastracture.ByEmail
{
    public class ConverterEmailsDtoTests
    {
        [Fact]
        public void ConverterEmailsDto_Should_Convert_To_Dto()
        {
            var sut = new ConverterEmailsDto();
            var element = new ResponseReserveDto();
            element.NameAvailability = "do 24h";
            element.WasSetsToZero = true;
            element.Symbol = "symbol";

            var actual = sut.GetNewNamesToFields(element);

            actual["NameAvailability"].Should().Be("<td>do 24h</td>");
            actual["WasSetsToZero"].Should().Be("<td>True</td>");
        }
    }
}
