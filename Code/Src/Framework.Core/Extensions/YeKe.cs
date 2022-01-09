using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Extensions
{
    public static class YeKe
    {
        public const char ArabicYeChar = (char)1610;
        public const char PersianYeChar = (char)1740;

        public const char ArabicKeChar = (char)1603;
        public const char PersianKeChar = (char)1705;

        public static string ApplyPersianYeKe(this string data)
        {
            return data == null ? null : (string.IsNullOrWhiteSpace(data) ?
                string.Empty :
                data.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim());
        }
    }
}
