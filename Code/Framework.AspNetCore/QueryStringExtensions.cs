namespace Framework.AspNetCore
{
    public static class QueryStringExtensions
    {
        public static T SafeGetValue<T>(this IQueryCollection collection, string key)
        {
            if (!collection.ContainsKey(key)) return default(T);

            var value = collection[key][0];
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
