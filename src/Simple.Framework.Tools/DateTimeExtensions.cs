namespace System;

public static class DateTimeExtensions
{
    public static bool IsDefault(this DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
        {
            return true;
        }

        var businessDefaultValue = new DateTime(1900, 1, 1);

        return dateTime <= businessDefaultValue;
    }

    public static string Format(this DateTime dateTime, TimeSpan? offset = null, bool ignoreTime = false)
    {
        if (dateTime.IsDefault())
        {
            return string.Empty;
        }

        if (offset.HasValue)
        {
            dateTime = dateTime.Add(offset.Value);
        }

        return ignoreTime ? dateTime.ToString("yyyy-MM-dd") : dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 时间格式化
    /// </summary>
    /// <param name="dateTime">时间</param>
    /// <param name="hasTime">格式化是否保留时分秒</param>
    /// <param name="addHours">追加的时区时间</param>
    /// <returns></returns>
    public static string ToDateTimeFormat(this DateTime dateTime,
        bool hasTime = true, int addHours = 8)
    {
        if (dateTime.IsDefault())
        {
            return string.Empty;
        }

        var utcTime = dateTime.ToUniversalTime();
        dateTime = utcTime.AddHours(addHours);

        return hasTime ?
            dateTime.ToString("yyyy-MM-dd HH:mm:ss") :
            dateTime.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// DateOnly转DateTime扩展
    /// </summary>
    /// <param name="dateOnly"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }

    public static (DateTime? start, DateTime? end) ToDateTimeRange(this IEnumerable<DateOnly>? dates)
    {
        var dateTimes = dates.ToDateTime();
        var startTime = dateTimes?.FirstOrDefault();
        var endTime = dateTimes?.LastOrDefault();

        return (
            startTime == null || startTime.Value.IsDefault() ? null : startTime,
            endTime == null || endTime.Value.IsDefault() ? null : endTime?.AddDays(1).AddHours(-8).AddMilliseconds(-1)
        );
    }
    public static IEnumerable<DateTime>? ToDateTime(this IEnumerable<DateOnly>? dates)
    {
        return dates?.Select(date => date.ToDateTime(new TimeOnly()));
    }

    public static List<DateOnly> ToDateOnly(DateTime? startTime, DateTime? endTime)
    {
        var dates = new List<DateOnly>();

        if (startTime != null)
        {
            dates.Add(DateOnly.FromDateTime((DateTime)startTime));
        }

        if (endTime != null)
        {
            dates.Add(DateOnly.FromDateTime((DateTime)endTime));
        }

        return dates;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="timezoneOffset"></param>
    /// <returns></returns>
    public static string ToDateAddHours(DateTime date, TimeSpan timezoneOffset)
    {
        return date.Add(timezoneOffset).ToString("yyyy-MM-dd");
    }

    /// <summary>  
    /// 将c# DateTime时间格式转换为Unix时间戳格式  
    /// </summary>  
    /// <param name="time">时间</param>  
    /// <returns>long</returns>         
    public static long ToTimeStamp(this DateTime time)
    {
        DateTimeOffset dto = new DateTimeOffset(time);
        return dto.ToUnixTimeMilliseconds();
    }
}