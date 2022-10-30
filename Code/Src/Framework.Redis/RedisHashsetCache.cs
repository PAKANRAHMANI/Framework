using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Redis;

public class RedisHashsetCache : IRedisHashsetCache
{
	private readonly IDatabase _database;
	private readonly RedisHashsetCacheConfiguration _redisCacheConfiguration;

	public RedisHashsetCache(IRedisDataBaseResolver redisHelper, CacheConfiguration cacheConfiguration)
	{
		this._redisCacheConfiguration = cacheConfiguration.RedisHashsetCacheConfiguration;
		this._database = redisHelper.GetDatabase(_redisCacheConfiguration.Connection, _redisCacheConfiguration.DbNumber);
	}

	public void HashSet<TKey, TValue>(string hashKey, Dictionary<TKey, TValue> data)
	{
		var fields = data.Select(pair => new HashEntry(pair.Key.ToString(), JsonConvert.SerializeObject(pair.Value))).ToArray();

		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		_database.HashSet(hashKey, fields);
	}

	public async Task HashSetAsync<TKey, TValue>(string hashKey, Dictionary<TKey, TValue> data)
	{
		var fields = data.Select(pair => new HashEntry(pair.Key.ToString(), JsonConvert.SerializeObject(pair.Value))).ToArray();

		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		await _database.HashSetAsync(hashKey, fields);
	}

	public void HashDelete(string hashKey, string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		_database.HashDelete(hashKey, key);
	}

	public async Task HashDeleteAsync(string hashKey, string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		await _database.HashDeleteAsync(hashKey, key);
	}

	public void HashSet(string key, object data)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		_database.HashSet(key, data.ToHashEntries());
	}
	public void HashSet(string key, string field, object data)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		var value = JsonConvert.SerializeObject(data);

		_database.HashSet(key, field, value);
	}
	public async Task HashSetAsync(string key, object data)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		await _database.HashSetAsync(key, data.ToHashEntries());
	}
	public async Task HashSetAsync(string key, string field, object data)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		var value = JsonConvert.SerializeObject(data);

		await _database.HashSetAsync(key, field, value);
	}
	public T HashGetAll<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		if (!_database.KeyExists(key))
			return default;

		var stockHashEntries = _database.HashGetAll(key);

		return stockHashEntries.ConvertFromRedis<T>();
	}
	public async Task<T> HashGetAllAsync<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		if (!_database.KeyExists(key))
			return default;

		var stockHashEntries = await _database.HashGetAllAsync(key);

		return stockHashEntries.ConvertFromRedis<T>();
	}
	public T HashGet<T>(string key, string fieldName)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		if (!_database.KeyExists(key))
			return default;

		var value = _database.HashGet(key, fieldName);
		if (!value.HasValue)
			return default(T);

		if ((typeof(T).IsClass || typeof(T).IsArray) && !(typeof(T)).IsString())
			return JsonConvert.DeserializeObject<T>(value);

		return (T)Convert.ChangeType(value, typeof(T));
	}
	public async Task<T> HashGetAsync<T>(string key, string fieldName)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		if (!await _database.KeyExistsAsync(key))
			return default;

		var value = await _database.HashGetAsync(key, fieldName);

		if (!value.HasValue)
			return default(T);

		if ((typeof(T).IsClass || typeof(T).IsArray) && !(typeof(T)).IsString())
			return JsonConvert.DeserializeObject<T>(value);

		return (T)Convert.ChangeType(value, typeof(T));
	}

	public List<object> HashKeys(string hashKey)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		var keys = _database.HashKeys(hashKey).ToList();

		return (List<object>)Convert.ChangeType(keys, typeof(List<object>));
	}

	public async Task<List<object>> HashKeysAsync(string hashKey)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		var keys = await _database.HashKeysAsync(hashKey);

		return (List<object>)Convert.ChangeType(keys.ToList(), typeof(List<object>));
	}

	public bool HashExist(string hashKey, string fieldName)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		return _database.HashExists(hashKey, fieldName);
	}

	public async Task<bool> HashExistAsync(string hashKey, string fieldName)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			hashKey = _redisCacheConfiguration.InstanceName + hashKey;

		return await _database.HashExistsAsync(hashKey, fieldName);
	}
}