using AutoMapper;
using ConsoleSetAvilability.Library.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public string ConvertToString(DateTime dateTime, string dateFormat)
        {
            return dateTime.ToString(dateFormat);
        }

        public DateTime ConvertToDateTime(string date, string dateFormat)
        {
            DateTime result = DateTime.Now;
            var sucsess =  DateTime.TryParseExact(date, dateFormat, new CultureInfo("pl-PL"), DateTimeStyles.None, out result);

            if (!sucsess)
                return DateTime.MinValue;

            return result;
        }
    }
}
