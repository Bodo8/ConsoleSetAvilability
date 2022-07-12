using ConsoleSetAvilability.Library.Infrastracture.Interfaces;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.ByEmail
{
    public class ConverterEmailsDto : IConverterEmailsDto
    {
        public  List<Dictionary<string, string>> GetConvertedList(List<ResponseReserveDto> ElementstsEmailDto)
        {
            var result = new List<Dictionary<string, string>>();

            foreach (var productDto in ElementstsEmailDto)
            {
                Dictionary<string, string> keyValuePairs = GetNewNamesToFields(productDto);

                result.Add(keyValuePairs);
            }

            return result;
        }

        public Dictionary<string, string> GetNewNamesToFields(ResponseReserveDto elementDto)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(elementDto);
            Dictionary<string, string>? dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dictionary != null)
            {
                foreach ( var item in dictionary)
                {
                    var propertyName = item.Key;

                    if (propertyName.Length > 0)
                    {
                        keyValuePairs.Add(propertyName, GetSingleValue(propertyName, elementDto));
                    }
                }

                return keyValuePairs;
            }

            return keyValuePairs;
        }

        private string GetSingleValue(string propertyEnglish, ResponseReserveDto element)
        {
            string output = "";
            string startHtml = $"<td>";
            string endHtml = "</td>";

            foreach (PropertyInfo propertyInfo in element.GetType().GetProperties())
            {
                string html = startHtml;

                if (propertyEnglish == propertyInfo.Name)
                {
                    var value = propertyInfo.GetValue(element)?.ToString();

                    return html + value + endHtml;
                }
            }
            return output;
        }
    }
}
