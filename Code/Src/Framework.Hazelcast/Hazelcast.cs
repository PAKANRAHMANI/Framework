using Hazelcast;
using Hazelcast.Serialization;
using Microsoft.Extensions.Options;

namespace Framework.Hazelcast;

public class Hazelcast : IHazelcast
{
    private readonly List<HazelcastSerialize> _serializes;
    private readonly IHazelcastClient _client;
    public Hazelcast(List<HazelcastSerialize> serializes,IOptions<HazelcastConfiguration> hazelcastConfiguration)
    {
        _serializes = serializes;
        _client = HazelcastClientFactory.StartNewClientAsync(config =>
        {
            config.ClusterName = hazelcastConfiguration.Value.ClusterName;
            config.Networking.Addresses.Add(hazelcastConfiguration.Value.NetworkingAddresses);
            config.ClientName = hazelcastConfiguration.Value.ClientName;
            config.Serialization.Serializers.Clear();
            foreach (var serialize in _serializes)
            {
                config.Serialization.Serializers.Add(new SerializerOptions()
                {
                    Creator = () => serialize.Object,
                    SerializedType = serialize.Type
                });
            }

        }).Result;
    }

    public async Task MapSetAsync<TKey, TValue>(TKey key, TValue value, string mapName)
    {
        await using var map = await _client.GetMapAsync<TKey, TValue>(mapName);

        await map.SetAsync(key, value);
    }

    public async Task<TValue> MapGetAsync<TKey, TValue>(TKey key, string mapName)
    {
        await using var map = await _client.GetMapAsync<TKey, TValue>(mapName);

        return await map.GetAsync(key);
    }
}