using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Redis;

public static class HashSetExtensions
{
	public static HashEntry[] ToHashEntries(this object obj)
	{
		var properties = obj.GetType().GetProperties();

		return properties
			.Where(x => x.GetValue(obj) != null)
			.Select
			(
				property =>
				{
					var propertyValue = property.GetValue(obj);
					string hashValue;

					if (property.PropertyType.IsClass || propertyValue is IEnumerable<object>)
					{
						hashValue = JsonConvert.SerializeObject(propertyValue);
					}
					else
					{
						hashValue = propertyValue.ToString();
					}

					return new HashEntry(property.Name, hashValue);
				}
			)
			.ToArray();
	}

	public static T ConvertFromRedis<T>(this HashEntry[] hashEntries)
	{
		var properties = typeof(T).GetProperties();

		var obj = Activator.CreateInstance(typeof(T), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
		foreach (var property in properties)
		{
			var entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
			if (entry.Equals(new HashEntry())) continue;

			var value =
				(property.PropertyType.IsClass || property.PropertyType.IsArray) && !property.PropertyType.IsString()
					? JsonConvert.DeserializeObject(entry.Value, property.PropertyType)
					: Convert.ChangeType(entry.Value, property.PropertyType);


			if (property.CanWrite == false && typeof(T).BaseType?.IsAbstract == true)
			{

				var baseProperty = property.DeclaringType?.GetProperty(property.Name);

				baseProperty?.GetSetMethod(true)?.Invoke(obj, new object[] { value });
			}
			else
			{
				property.SetValue(obj, value, BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
			}
		}

		return (T)obj;
	}

	public static bool IsString(this Type type)
	{
		return type == typeof(string);
	}
}
