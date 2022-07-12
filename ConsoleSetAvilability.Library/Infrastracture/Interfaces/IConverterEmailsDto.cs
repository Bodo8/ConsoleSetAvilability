using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.Interfaces
{
    public interface IConverterEmailsDto
    {
        List<Dictionary<string, string>> GetConvertedList(List<ResponseReserveDto> value);
    }
}
