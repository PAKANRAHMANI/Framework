using System;
using System.Globalization;

namespace Framework.Utility;

public static class DatetTimeExtentions
{
    public static string ToShamsiDate(this DateTime dateTime, bool convertToLocalTime = true)
    {
        if (convertToLocalTime)
            dateTime = dateTime.ToLocalTime();

        var persianCalendar = new PersianCalendar();

        var intYear = persianCalendar.GetYear(dateTime);
        var intMonth = persianCalendar.GetMonth(dateTime);
        var intDay = persianCalendar.GetDayOfMonth(dateTime);
        var intHour = persianCalendar.GetHour(dateTime);
        var intMinutes = persianCalendar.GetMinute(dateTime);
        var intSeconds = persianCalendar.GetSecond(dateTime);

        var strYear = intYear.ToString();
        var strMonth = intMonth < 10 ? "0" + intMonth.ToString() : intMonth.ToString();
        var strDay = intDay < 10 ? "0" + intDay.ToString() : intDay.ToString();
        var strHour = intHour < 10 ? "0" + intHour.ToString() : intHour.ToString();
        var strMinutes = intMinutes < 10 ? "0" + intMinutes.ToString() : intMinutes.ToString();
        var strSeconds = intSeconds < 10 ? "0" + intSeconds.ToString() : intSeconds.ToString();

        return ($"{strYear}/{strMonth}/{strDay} {strHour}:{strMinutes}:{strSeconds}");
    }
    public static string ToShamsiDateWithoutTime(this DateTime dateTime, bool convertToLocalTime = true)
    {
        if (convertToLocalTime)
            dateTime = dateTime.ToLocalTime();

        var persianCalendar = new PersianCalendar();

        var intYear = persianCalendar.GetYear(dateTime);
        var intMonth = persianCalendar.GetMonth(dateTime);
        var intDay = persianCalendar.GetDayOfMonth(dateTime);

        var strYear = intYear.ToString();
        var strMonth = intMonth < 10 ? "0" + intMonth.ToString() : intMonth.ToString();
        var strDay = intDay < 10 ? "0" + intDay.ToString() : intDay.ToString();

        return ($"{strYear}/{strMonth}/{strDay}");
    }
    /// <summary>
    /// return datetime with format 2022/02/02 02:02:02
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="convertToLocalTime"></param>
    /// <returns>dateTime with format 2022/02/02 02:02:02</returns>
    public static string ToShortDate(this DateTime dateTime, bool convertToLocalTime = true)
    {
        if (convertToLocalTime)
            dateTime = dateTime.ToLocalTime();
        return $"{dateTime:yyyy/MM/dd hh:mm:ss}";
    }
}