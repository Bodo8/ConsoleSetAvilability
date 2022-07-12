using ConsoleSetAvilability.Library.Infrastracture.Interfaces;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Infrastracture.ByEmail
{
    public class EmailComposer : IEmailComposer
    {
        private readonly IConverterEmailsDto _converterEmails;

        public EmailComposer(IConverterEmailsDto converterEmails)
        {
            _converterEmails = converterEmails;
        }

        public string ComposeTableWithHtml(Dictionary<string, List<ResponseReserveDto>> elements)
        {
            string css = "table { border - collapse: collapse; border: 1px solid black; }" +
                         "table caption { font-weight: bold; padding: 10px; }" +
                         "table th, table td { border: 1px solid black; padding: 5px 10px; }";

            var tables =  CreateTables(elements);

            var result = $"<html><head><style type=\"text/css\">{css}</style></head><body>{tables}</body></html>";
            return result;
        }

        public string CreateTables(Dictionary<string, List<ResponseReserveDto>> elements)
        {
            string output = "";

            foreach (var element in elements)
            {
                List<Dictionary<string, string>> listKeyValuePairs = _converterEmails.GetConvertedList(element.Value);

                if (element.Value.Count > 0)
                {
                    var header = CreateHeader(listKeyValuePairs.First().Keys);
                    var caption = CreateCaption(element.Key);
                    var content = CreateContent(listKeyValuePairs);

                    output += $"<table>{caption}{header}{content}</table>";
                }
            }

            return output;
        }
        public string CreateCaption(string caption)
        {
            return $"<caption>Zmiana cen dla: {caption}</caption>";
        }

        public string CreateHeader(Dictionary<string, string>.KeyCollection element)
        {
            string header = "";
            foreach (var item in element)
            {
                header += $"<th>{item}</th>";
            }
            return $"<tr>{header}</tr>";
        }

        public string CreateContent(List<Dictionary<string, string>> elements)
        {
            string output = "";
            foreach (var Item in elements)
            {
                output += CreateSingleContentRow(Item);
            }
            return output;
        }

        public string CreateSingleContentRow(Dictionary<string, string> data)
        {
            string output = "";
            foreach (var value in data)
            {
                output += value.Value;
            }
            return $"<tr>{output}</tr>";
        }
    }
}
