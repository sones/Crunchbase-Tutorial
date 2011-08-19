using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

namespace Crunchbase.ConnectingNodes.Connections
{
    internal static class StringExtensions
    {
        internal static String StringFormat(this String s, params object[] args)
        {
            return string.Format(s, args);
        }

        internal static String GetKeyValueString<T>(this T value, String key)
        {
            return GetKeyXString("{1} = {0}", value, key);
        }

        internal static String GetKeyRefString<T>(this T value, String key, String refKey)
        {
            return GetKeyXString("{1} = REF({2} = {0})", value, key, refKey);
        }

        internal static String StringWithComma(this String s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            return ", " + s;
        }

        internal static String GetSETOF<V>(this IEnumerable<V> links, String attributeName, String refKey, String sign = "=")
        {
            if (links != null && links.Count(x => x != null) > 0)
                return "{0} {2} SETOF({1})".StringFormat(attributeName, links.Aggregate(String.Empty, (agg, next) => agg + ", " + next.ToString().GetKeyValueString(refKey)).Substring(2), sign);

            return String.Empty;
        }

        internal static String GetSETOF<V>(this V link, String attributeName, String refKey, String sign = "=")
        {
            return (new V[] { link } as IEnumerable<V>).GetSETOF(attributeName, refKey, sign);
        }

        private static String GetKeyXString<T>(String template, T value, params object[] extraArgs)
        {
            if (value == null)
                return string.Empty;

            if (value == null)
                return string.Empty;



            var args = extraArgs.ToList();
            if (value.GetType() == typeof(String))
                args.Insert(0, "'" + value.ToString().Replace('\n', ' ').Replace('\r', ' ').Replace("'", "''").Replace("\\", " ") + "'");
            else
                if (value.GetType() == typeof(DateTime))
                    args.Insert(0, "'" + (value as Nullable<DateTime>).Value.ToString(CultureInfo.GetCultureInfo("en-US")) + "'");
                else if (value.GetType() == typeof(double))
                    args.Insert(0, (value as Nullable<double>).Value.ToString(CultureInfo.GetCultureInfo("en-US")));
                else if (value.GetType() == typeof(float))
                    args.Insert(0, (value as Nullable<float>).Value.ToString(CultureInfo.GetCultureInfo("en-US")));
                else if (value.GetType() == typeof(decimal))
                    args.Insert(0, (value as Nullable<decimal>).Value.ToString(CultureInfo.GetCultureInfo("en-US")));
                else
                    args.Insert(0, value);

            return template.StringFormat(args.ToArray());
        }
    }

}
