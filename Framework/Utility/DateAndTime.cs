using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.Utility
{
    public class DateAndTime
    {
        public static DateTime NoValueDate = new DateTime(1900, 1, 1);
        public static string ShortDateFormat = "dd/MM/yyyy";

        public static string DisplayShortDate(DateTime Date)
        {
            if (Date == NoValueDate) return string.Empty;
            return Date.ToString(ShortDateFormat);
        }

        public static string DisplayShortDate(DateTime Date, string DefaultText)
        {
            if (Date == NoValueDate) return DefaultText;
            return Date.ToString(ShortDateFormat);
        }

        public static string DisplayShortTimeString(int Minutes)
        {
            return string.Format("{0}:{1:D2}", Minutes / 60, Minutes % 60);
        }
        public static string DisplayDay(DateTime Date)
        {
            if (Date == NoValueDate) return string.Empty;
            return Date.DayOfWeek.ToString();
        }
        public static string DisplayLongTimeString(int Minutes)
        {
            if (Minutes < 60)
            {
                return string.Format("{0} min", Minutes);
            }
            else if (Minutes >= 60 && Minutes % 60 == 0)
            {
                return string.Format("{0} hr{1}", Minutes / 60, Minutes == 60 ? "s" : string.Empty);
            }
            else
            {
                return string.Format("{0} hr{1} {2} min", Minutes / 60, Minutes < 120 ? "s" : string.Empty, Minutes % 60);
            }
        }


        //There is a copy of this method in DOBase (for setting date on created objects) so change the method there also if this one needs changing.
        public static DateTime GetCurrentDateTime()
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
        }
    }
}
