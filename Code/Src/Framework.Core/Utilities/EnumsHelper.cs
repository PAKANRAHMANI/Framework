using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Utilities
{
    public static class EnumsHelper
    {

    
      
        public static string GetDescription<TEnum>(TEnum value)
            where TEnum : Enum
        {
            return Cache<TEnum>.DescriptionCollection.TryGetValue(value, out var desciption)
                ? desciption
                : value.ToString();
        }



        private static class Cache<TEnum>
            where TEnum : Enum
        {
            private const string NotImportantTitle = "NotImportant";
            public static readonly HashSet<TEnum> Values;
            public static readonly Dictionary<string, TEnum> Names;
            public static readonly TEnum NotImportantValue;
            public static readonly bool IsDefinedNotImportant;
            public static readonly HashSet<TEnum> MainValues;   //all values excluding the 'NotImportant'
            public static readonly Dictionary<TEnum, string> DescriptionCollection;

            static Cache()
            {
                Values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToHashSet();

                Names = new Dictionary<string, TEnum>(Values.Count, StringComparer.OrdinalIgnoreCase);
                foreach (var item in Values)
                {
                    Names.Add(item.ToString(), item);
                }

                if (Names.TryGetValue(NotImportantTitle, out var notImportant))
                {
                    int valueNumber = (int)(object)notImportant;
                    if (valueNumber == 0)
                    {
                        NotImportantValue = notImportant;
                        IsDefinedNotImportant = true;
                    }
                }

                if (IsDefinedNotImportant)
                {
                    MainValues = Values.ToHashSet();
                    MainValues.Remove(NotImportantValue);
                }
                else
                {
                    MainValues = Values;
                }

                DescriptionCollection = GetDescriptionCollection();
            }

            private static Dictionary<TEnum, string> GetDescriptionCollection()
            {
                var collection = new Dictionary<TEnum, string>();

                var type = typeof(TEnum);
                var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

                foreach (var value in values)
                {
                    var description = value.ToString();
                    var fieldInfo = type.GetField(description);

                    if (fieldInfo != null)
                    {
                        var attribute = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), false);
                        if (attribute != null)
                        {
                            description = ((DescriptionAttribute)attribute).Description;
                        }
                    }

                    collection.TryAdd(value, description);
                }

                return collection;
            }
        }
    }
}
