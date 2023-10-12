﻿namespace Framework.Caching.Strategy;

public interface ICache
{
    T Get<T>(string key, int? expirationTimeInMinutes = null) where T : class;
    void Set<T>(string key, T value, int expirationTimeInMinutes) where T : class;
}