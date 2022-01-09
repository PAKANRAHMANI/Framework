﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Framework.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IDistributedCache _cache;

        public RedisCache(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T GetString<T>(string key)
        {
            var value = _cache.GetString(key);

            if (value == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> GetStringAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public void SetString(string key, object data, int expirationTimeInMinutes)
        {
            var value = JsonConvert.SerializeObject(data);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiresIn);

            _cache.SetString(key, value, options);
        }

        public async Task SetStringAsync(string key, object data, int expirationTimeInMinutes)
        {
            var value = JsonConvert.SerializeObject(data);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiresIn);

            await _cache.SetStringAsync(key, value, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public bool TryGetValue<T>(string key, out T obj)
        {
            var value = _cache.GetString(key);

            if (value == null)
            {
                obj = default(T);
                return false;
            }

            obj = JsonConvert.DeserializeObject<T>(value);

            return true;
        }

        public async Task Set(string key, IEnumerable<object> data, int expirationTimeInMinutes)
        {
            var value = JsonConvert.SerializeObject(data);

            var expiresIn = TimeSpan.FromMinutes(expirationTimeInMinutes);

            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiresIn);
            
            await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task<List<T>> GetAsync<T>(string key)
        {
            var value = await _cache.GetAsync(key);

            if (value == null)
                return default;

            return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(value));
        }

        public List<T> Get<T>(string key)
        {
            var value = _cache.Get(key);

            if (value == null)
                return default;

            return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(value));
        }
    }
}