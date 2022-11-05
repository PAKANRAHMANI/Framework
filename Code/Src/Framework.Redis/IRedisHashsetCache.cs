using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Redis;

public interface IRedisHashsetCache
{
	void HashSet(string key, object data);
	Task HashSetAsync(string key, object data);
	void HashSet(string key, string field, object data);
	Task HashSetAsync(string key, string field, object data);
	void HashSet<TKey, TValue>(string hashKey, Dictionary<TKey, TValue> data);
	Task HashSetAsync<TKey, TValue>(string hashKey, Dictionary<TKey, TValue> data);
	void HashDelete(string hashKey, string key);
	Task HashDeleteAsync(string hashKey, string key);
	T HashGetAll<T>(string key);
	Task<T> HashGetAllAsync<T>(string key);
	T HashGet<T>(string key, string fieldName);
	Task<T> HashGetAsync<T>(string key, string fieldName);
	List<object> HashKeys(string hashKey);
	Task<List<object>> HashKeysAsync(string hashKey);
	bool HashExist(string hashKey, string fieldName);
	Task<bool> HashExistAsync(string hashKey, string fieldName);
}
