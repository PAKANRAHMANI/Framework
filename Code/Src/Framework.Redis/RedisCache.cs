﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Exceptions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Framework.Redis;

public class RedisCache : IRedisCache
{
	private readonly IDatabase _database;
	private readonly RedisCacheConfiguration _redisCacheConfiguration;

	public RedisCache(IRedisDataBaseResolver redisHelper, CacheConfiguration cacheConfiguration)
	{
		this._redisCacheConfiguration = cacheConfiguration.RedisCacheConfiguration;
		this._database = redisHelper.GetDatabase(_redisCacheConfiguration.Connection, _redisCacheConfiguration.DbNumber);
	}

	public void Set(string key, object data, int? expirationTimeInMinutes)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;
		var value = JsonConvert.SerializeObject(data);

		_database.StringSet(key, value, expirationTimeInMinutes != null ? TimeSpan.FromMinutes(expirationTimeInMinutes.Value) : null);
	}

	public async Task SetAsync(string key, object data, int? expirationTimeInMinutes)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		var value = JsonConvert.SerializeObject(data);

		await _database.StringSetAsync(key, value, expirationTimeInMinutes != null ? TimeSpan.FromMinutes(expirationTimeInMinutes.Value) : null);
	}

	public T Get<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		string value = _database.StringGet(key);
		if (value == null)
			return default;

		var obj = JsonConvert.DeserializeObject<T>(value);
		if (obj == null)
			return default;

		return obj;
	}

	public IEnumerable<T> GetValues<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		string value = _database.StringGet(key);
		if (value == null)
			return default;

		var list = JsonConvert.DeserializeObject<List<T>>(value);
		if (list == null || !list.Any())
			return default;
		return list;
	}

	public async Task<T> GetAsync<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		string value = await _database.StringGetAsync(key);

		return value == null ? default : JsonConvert.DeserializeObject<T>(value);
	}

	public bool TryGetValue<T>(string key, out T obj)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		string value = _database.StringGet(key);

		if (value == null)
		{
			obj = default(T);
			return false;
		}

		obj = JsonConvert.DeserializeObject<T>(value);

		return true;
	}

	public async Task<IEnumerable<T>> GetValuesAsync<T>(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		var value = await _database.StringGetAsync(key);

		return value.HasValue ? JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(value)) : default(List<T>);
	}

	public void Remove(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;
		_database.KeyDelete(key);
	}

	public async Task RemoveAsync(string key)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		await _database.KeyDeleteAsync(key);
	}

	public bool LockRelease(string key, RedisValue value, CommandFlags flags = CommandFlags.None)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		return _database.LockRelease(key, value, flags);
	}

	public bool LockTake(string key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		return _database.LockTake(key, value, expiry, flags);
	}

	public bool KeyExists(string key, CommandFlags flags = CommandFlags.None)
	{
		if (_redisCacheConfiguration.UseFromInstanceNameInKey)
			key = _redisCacheConfiguration.InstanceName + key;

		return _database.KeyExists(key, flags);
	}
}
