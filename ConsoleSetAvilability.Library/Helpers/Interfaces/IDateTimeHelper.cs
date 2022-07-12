using System;

namespace ConsoleSetAvilability.Library.Helpers.Interfaces
{
    public interface IDateTimeHelper
    {
        DateTime ConvertToDateTime(string date, string dateFormat);
        string ConvertToString(DateTime dateTime, string dateFormat);
    }
}