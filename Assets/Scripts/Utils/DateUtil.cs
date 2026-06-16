using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

internal class DateUtil
{
	public static double time
    {
        get
        {
#if UnityTime
            
            return (double)Time.time;
#else
            return NetworkTime.time;
#endif

        }
    }

    /************************************************************************/
    /* 格式化时间：mm：ss                                               */
    /************************************************************************/

    public static string FormatMinAndSec(int totalSec)
    {
        TimeSpan ts = TimeSpan.FromSeconds(totalSec);
        return (ts.Minutes < 10 ? "0" + ts.Minutes.ToString() : ts.Minutes.ToString()) + ":" + 
            (ts.Seconds < 10 ? "0" + ts.Seconds.ToString() : ts.Seconds.ToString());
    }

    /************************************************************************/
    /* 秒转时间戳。（1970-1-1为标准）                                               */
    /************************************************************************/

    public static DateTime ConvertDateTime(double seconds)
    {
        DateTime dt1970 = new DateTime(1970, 1, 1);
        return dt1970.AddSeconds(seconds).ToLocalTime();
    }

    /************************************************************************/
    /* 格式化时间：hh：mm：ss                                               */
    /************************************************************************/

    public static string FormatHourAndMinAndSec(int totalSec)
    {
        TimeSpan ts = TimeSpan.FromSeconds(totalSec);
        return ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
    }

    /// <summary>
    /// Formats the hour and minimum and sec cn.
    /// </summary>
    /// <param name="totalSec">The total sec.</param>
    /// <returns>System.String.</returns>
    public static string FormatHourAndMinAndSecCN(int totalSec)
    {
        TimeSpan ts = TimeSpan.FromSeconds(totalSec);
        return ts.Hours.ToString() + "时" + ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒";
    }

    /************************************************************************/
    /* 格式化时间：hh：mm | 昨天 | 2天前                                        */
    /************************************************************************/

    public static string FormatHourAndMin(long timeStamp)
    {
        int date0 = GetDayTime(0);
        if (timeStamp > date0)
        {
            DateTime time = GetTime(timeStamp);
            return time.ToString("HH:mm");
        }
        int date1 = GetDayTime(-1);
        if (timeStamp > date1)
        {
            return "昨天";
        }
        else
        {
            return "2天前";
        }
    }

    public static string FormatMinAndSec_zero(long timesteamp)
    {
        DateTime time = GetTime(timesteamp);
        return time.ToString("HH:mm");
    }

    public static DateTime GetTime(long timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp.ToString() + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    private static int GetDayTime(int day)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime dtNow = DateTime.Parse(DateTime.Now.AddDays(day).ToString("yyyy-MM-dd"));
        TimeSpan toNow = dtNow.Subtract(dtStart);
        string timeStamp = toNow.Ticks.ToString();
        return int.Parse(timeStamp.Substring(0, timeStamp.Length - 7));
    }

    /************************************************************************/
    /* 获得UTC当前秒数                                                      */
    /************************************************************************/

    public static int NowSec
    {
        get
        {
            //return (int)DateTime.Now.ToFileTimeUtc() / 10000000;

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            int now = int.Parse(timeStamp.Substring(0, timeStamp.Length - 7));
            return now;
        }
    }

    public static long NowMllSec
    {
        get
        {
            //return (int)DateTime.Now.ToFileTimeUtc() / 10000000;

//            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
//            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
//            TimeSpan toNow = dtNow.Subtract(dtStart);
//            string timeStamp = toNow.Ticks.ToString();
//            long now = long.Parse(timeStamp.Substring(0, timeStamp.Length - 4));
//            return now;

			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalMilliseconds);
        }
    }

	public static long getTimeStamp()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalMilliseconds);
	}

    private static string replaceDateStr(int value)
    {
        if (value.ToString().Length <= 1)
        {
            return "0" + value.ToString();
        }
        else
        {
            return value.ToString();
        }
    }

    private static long serverTimeDiff = 0;

    public static long ServerTimeDiff
    {
        set
        {
            serverTimeDiff = value;
            Debug.Log("serverTimeDiff : " + serverTimeDiff);
        }
        get
        {
            return serverTimeDiff;
        }
    }

    public static long ServerTime
    {
        get
        {
            return NowMllSec - serverTimeDiff;
        }
    }

    public static string CurDateTimeStr()
    {
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;
        int second = DateTime.Now.Second;
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        return string.Format("{0:D2}:{1:D2}:{2:D2} " + "{3:D4}/{4:D2}/{5:D2}", hour, minute, second, year, month, day);
    }
}