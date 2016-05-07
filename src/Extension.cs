using System;

namespace SmartLog
{
    public static class Extension
    {
        public static bool IsNotNull(this object value)
        {
            return value != null && value != DBNull.Value;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return IsNotNull(value) & !value.IsNullOrEmpty();
        }

        public static string GetValueOrDefault<T>(this T value, string defaultValue)
        {
            var result = IsNotNull(value) ? value.ToString() : default(string);
            return !result.IsNullOrEmpty() ? result : defaultValue;
        }

        public static int ToInt<T>(this T value, int defaultValue = default(int))
        {
            int result = defaultValue;
            if (value.IsNotNull())
            {
                int.TryParse(value.ToString(), out result);
            }
            return result;
        }

        public static string FormatEx(this string value, params object[] args)
        {
            return string.Format(value, args);
        }
    }
}
