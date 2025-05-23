﻿namespace Framework.Caching.Extensions.Abstractions;

public interface ICacheControl
{
    void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class;
    void Set<T>(string key, T value) where T : class;
    T Get<T>(string key) where T : class;
    void Remove(string key);
}
