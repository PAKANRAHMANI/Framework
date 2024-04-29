using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Utilities;

namespace Framework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? "";
        }
        public static string GetDescription<TEnum>(this TEnum value)
            where TEnum : Enum
        {
            return EnumsHelper.GetDescription(value);

        }

        public static string GetDescription<TEnum>(this TEnum? nullableValue)
            where TEnum : struct, Enum
        {
            return nullableValue.HasValue
                ? nullableValue.Value.GetDescription()
                : null;
        }

        public static bool Any<T>(this T source, params T[] items)
            where T : Enum
        {
            if (source != null && items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (source.Equals(items[i]))
                        return true;
                }
            }

            return false;
        }

        public static bool Any<T>(this T? source, params T[] items)
            where T : struct, Enum
        {
            return source.HasValue && source.Value.Any(items);
        }

        public static bool NotAny<T>(this T source, params T[] items)
            where T : Enum
        {
            return !source.Any(items);
        }

        public static bool NotAny<T>(this T? source, params T[] items)
            where T : struct, Enum
        {
            return !source.Any(items);
        }
    }
}
