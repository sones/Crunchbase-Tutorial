using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Globalization;
using System.IO;

namespace Crunchbase.Model
{


    public static class JSONStringExtension
    {

        /**
         * <summary>Extends string to convert the JSON content into a class.</summary>
         */
        public static T DeserializeJSON<T>(this string json) where T : class
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ser.MaxJsonLength = 20000000;
                return ser.Deserialize<T>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    static class StringExtension
    {
        public static DateTime? AsDateTime(this string s)
        {
            try
            {
                return DateTime.ParseExact(s, "ddd MMM d H:mm:ss \"UTC\" yyyy", new CultureInfo("en-US"));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    static class DateTimeHelper
    {
        public static DateTime? getIntegersAsDateTime(int? year, int? month, int? day)
        {
            if (year == null)
                return null;

            if (month == null)
                month = 1;
            if (day == null)
                day = 1;

            return new DateTime(year.Value, month.Value, day.Value);
        }
    }
}
