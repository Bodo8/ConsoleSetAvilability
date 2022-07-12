using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleSetAvilability.Library.Infrastracture.Interfaces
{
    public interface IEmailComposer
    {
        string ComposeTableWithHtml(Dictionary<string, List<ResponseReserveDto>> websitesToUpdate);
    }
}
