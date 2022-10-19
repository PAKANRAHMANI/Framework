using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Framework.Redis
{
    public interface IRedisCache
    {
        void HashSet(string key, object data);
        void HashSet(string key, string field, object data);
        Task HashSetAsync(string key,object data);
        Task HashSetAsync(string key, string field, object data);
        T HashGetAll<T>(string key);
        Task<T> HashGetAllAsync<T>(string key);
        T HashGet<T>(string key, string fieldName);
        Task<T> HashGetAsync<T>(string key, string fieldName);
        void Set(string key, object data, int expirationTimeInMinutes);
        Task SetAsync(string key, object data, int expirationTimeInMinutes);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        IEnumerable<T> GetValues<T>(string key);
        Task<IEnumerable<T>> GetValuesAsync<T>(string key);
        bool TryGetValue<T>(string key, out T result);
        void Remove(string key);
        Task RemoveAsync(string key);

        /// <summary>
        /// Releases a lock, if the token value is correct.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value at the key that must match.</param>
        /// <param name="flags">The flags to use for this operation.</param>
        /// <returns><see langword="true"/> if the lock was successfully released, <see langword="false"/> otherwise.</returns>
        bool LockRelease(string key, RedisValue value, CommandFlags flags = CommandFlags.None);

        /// <summary>
        /// Takes a lock (specifying a token value) if it is not already taken.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value to set at the key.</param>
        /// <param name="expiry">The expiration of the lock key.</param>
        /// <param name="flags">The flags to use for this operation.</param>
        /// <returns><see langword="true"/> if the lock was successfully taken, <see langword="false"/> otherwise.</returns>
        bool LockTake(string key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None);
        //
        // Summary:
        //     Returns if key exists.
        //
        // Parameters:
        //   key:
        //     The key to check.
        //
        //   flags:
        //     The flags to use for this operation.
        //
        // Returns:
        //     true if the key exists. false if the key does not exist.
        //
        // Remarks:
        //     https://redis.io/commands/exists
        bool KeyExists(string key, CommandFlags flags = CommandFlags.None);
    }
}