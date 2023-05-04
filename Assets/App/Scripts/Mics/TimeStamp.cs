using System;
using System.Globalization;
using UnityEngine;

public static class TimeStamp
{
    public static DateTime EpochUTC   = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static DateTime EpochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
    public static DateTime FirstDay = new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 1/1/0001 12:00:00 AM
    /// 0001-01-01 00:00:00
    /// start of the Gregorian calendar
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static bool IsFirstDay(DateTime time)
    {
        return (new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc) - time).TotalSeconds == 0;
    }
    
    public static bool IsFirstDay(string time)
    {
        var firstDayDT = DateTime.Parse(time);
        return IsFirstDay(firstDayDT);
    }
    
    public static long SecondUTC()
    {
        return (long) (DateTime.UtcNow - EpochUTC).TotalSeconds;
    }

    public static long Second()
    {
        return (long) (DateTime.Now - EpochLocal).TotalSeconds;
    }

    public static long MillisecondUTC()
    {
        return (long) (DateTime.UtcNow - EpochUTC).TotalMilliseconds;
    }

    public static long SecondUTC(DateTime dateTime)
    {
        return (long) (dateTime - EpochUTC).TotalSeconds;
    }

    public static long Second(DateTime dateTime)
    {
        return (long) (dateTime - EpochLocal).TotalSeconds;
    }

    public static long TimeFromString(string timeString, string format)
    {
        try
        {
            DateTime dt       = DateTime.ParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            long     valueUTC = SecondUTC(dt);
            return valueUTC;
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public static DateTime DateTimeFromString(string timeString, string format)
    {
        try
        {
            DateTime dt = DateTime.ParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            return dt;
        }
        catch (Exception e)
        {
            return DateTime.Now;
        }
    }

    public static DateTime DateTimeFromLocalSeconds(long seconds)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(seconds);
    }

    public static DateTime DateTimeFromSeconds(long seconds)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
    }

    public static long StartDayTimeUTC()
    {
        DateTime dateTime = DateTime.UtcNow.StartOfDayUTC();
        return (long) (dateTime - EpochUTC).TotalSeconds;
    }

    public static long EndDayTimeUTC()
    {
        DateTime dateTime = DateTime.UtcNow.EndOfDayUTC();
        return (long) (dateTime - EpochUTC).TotalSeconds;
    }

    public static DateTime EndOfDayUTC(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, DateTimeKind.Utc);
    }

    public static DateTime StartOfDayUTC(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, DateTimeKind.Utc);
    }

    public static string GetTimeUntilTo(long nextTime, bool toLowerCase = false)
    {
        DateTime now  = DateTime.UtcNow;
        DateTime next = new DateTime(1970, 1, 1, 0, 0, 00, DateTimeKind.Utc).AddSeconds(nextTime);

        string text   = "00:00";
        long   remain = (long) ((next - now).TotalSeconds);
        if (remain > 0)
        {
            text = GetTimeString(remain);
        }
        if (toLowerCase)
        {
            text = text.ToLower();
        }
        return text;
    }

    public static string GetTimeUntilTo(DateTime nextTime, bool toLowerCase = false)
    {
        DateTime now    = DateTime.UtcNow;
        string   text   = "00:00";
        long     remain = (long) ((nextTime - now).TotalSeconds);
        if (remain > 0)
        {
            text = GetTimeString(remain);
        }
        if (toLowerCase)
        {
            text = text.ToLower();
        }
        return text;
    }

    public static long GetTimeRemain(DateTime nextTime)
    {
        return (long) ((nextTime - DateTime.UtcNow).TotalSeconds);
    }

    /*public static string GetTimeString(long seconds)
    {
        if (seconds > 60 * 60) //convert to hours
        {
            float hours = (float) seconds / (float) (60 * 60);
            return Localize.Format("{0} hours", Mathf.FloorToInt(hours + 0.5f));
        }
        else if (seconds > 60) //convert to munites
        {
            float munites = (float) seconds / (float) 60;
            return Localize.Format("{0} minutes", Mathf.FloorToInt(munites + 0.5f));
        }
        else
        {
            return Localize.Format("{0} seconds", seconds);
        }
    }*/

    public static string GetTimeString(long timeInSeconds)
    {
        if (timeInSeconds > 86400) //convert to day => show day + hour
        {
            int days  = (int) (timeInSeconds / 86400);
            int hours = Mathf.FloorToInt((float) (timeInSeconds - days * 86400) / 3600 /*+ 0.5f*/); //round up final value: 0.5 hour => will show as 1 hour 
            return $"{days:D2}d{hours:D2}h";
        }
        else if (timeInSeconds > 3600) //convert to hours => show hour + minutes
        {
            int hours   = (int) (timeInSeconds / 3600);
            int minutes = Mathf.FloorToInt(((float) timeInSeconds - hours * 3600) / 60 /*+ 0.5f*/);
            return $"{hours:D2}h{minutes:D2}m";
        }
        else if (timeInSeconds > 60) //convert to minutes => show minutes + seconds
        {
            int minutes = (int) (timeInSeconds / 60);
            int seconds = (int) (timeInSeconds % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
        else
        {
            return $"00:{timeInSeconds:D2}";
        }
    }

    public static string GetTimerString(long timeInSeconds)
    {
        int munites = (int) (timeInSeconds / 60);
        int seconds = (int) (timeInSeconds % 60);
        return string.Format("{0:D2}:{1:D2}", munites, seconds);
    }

    public static string GetTimerString_H_M_S(long timeInSeconds)
    {
        int hours   = (int) (timeInSeconds / (60 * 60));
        int munites = (int) ((timeInSeconds - hours * (60 * 60)) / 60);
        int seconds = (int) (timeInSeconds % 60);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, munites, seconds);
    }

    public static long ConvertLocalToUTC(long localTimeStamp)
    {
        DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        date = date.AddSeconds((int) localTimeStamp);
        DateTime nowUTC = TimeZoneInfo.ConvertTimeToUtc(date, TimeZoneInfo.Local);
        return (long) (nowUTC - EpochUTC).TotalSeconds;
    }

    public static int CompareDayVsDay(long timeStamp1, long timeStamp2)
    {
        DateTime date_1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        date_1 = date_1.AddSeconds((int) timeStamp1);

        DateTime date_2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        date_2 = date_2.AddSeconds((int) timeStamp2);

        if (date_1.Year > date_2.Year) { return 1; }
        else if (date_1.Year < date_2.Year) { return -1; }
        else
        {
            if (date_1.Month > date_2.Month) { return 1; }
            else if (date_1.Month < date_2.Month) { return -1; }
            else
            {
                if (date_1.Date > date_2.Date) { return 1; }
                else if (date_1.Date < date_2.Date) { return -1; }
                else { return 0; }
            }
        }
    }
}