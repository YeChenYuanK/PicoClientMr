using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LekeNet.Util
{
    public class DateHelper
    {

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

    }
}
