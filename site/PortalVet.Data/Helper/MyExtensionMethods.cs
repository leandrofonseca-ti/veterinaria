using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalVet.Data.Helper
{
    public static class MyExtensionMethods
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string ToUTF8(this string value)
        {
            var utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(value);
            value = utf8.GetString(utfBytes, 0, utfBytes.Length);

            return value;
        }

        public static int ToInteger(this string value)
        {
            int.TryParse(value, out int temp);

            return temp;
        }

        public static Int64 ToLong(this string value)
        {
            Int64.TryParse(value, out Int64 temp);

            return temp;
        }

        public static double ToDouble(this string value)
        {
            double.TryParse(value, out double temp);

            return temp;
        }

        public static decimal ToDecimal(this string value)
        {
            decimal.TryParse(value, out decimal temp);

            return temp;
        }

        public static decimal ToDecimal(this string value, int casasDecimais)
        {
            decimal.TryParse(value, out decimal temp);

            return Math.Round(temp, casasDecimais);
        }

        public static DateTime ToDateTimeSuperLogica(this string value)
        {
            var dateTemp = value.Split('/');
            var dateTemp2 = $"{dateTemp[1]}/{dateTemp[0]}/{dateTemp[2]}";

            DateTime.TryParse(dateTemp2, out DateTime temp);

            return temp;
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime.TryParse(value, out DateTime temp);

            if (temp <= new DateTime(1900, 1, 1))
            {
                temp = new DateTime(1900, 1, 1);
            }

            return temp;
        }

        public static DateTime? ToDateTimeAngularJS(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains("GMT"))
                {
                    var dados = value.Split("GMT".ToCharArray());
                    value = dados[0];
                }

                DateTime.TryParse(value, out DateTime temp);

                if (temp <= new DateTime(1900, 1, 1))
                {
                    temp = new DateTime(1900, 1, 1);
                }

                return temp;
            }

            return null;
        }

        public static DateTime ToDateTime2(this string value)
        {
            DateTime.TryParse(value, out DateTime temp);

            return temp;
        }

        public static bool ToBoolean(this string value)
        {
            bool.TryParse(value, out bool temp);

            return temp;
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            sbReturn = sbReturn.Replace("º", string.Empty).Replace("ª", string.Empty);

            return sbReturn.ToString();
        }
    }
}
