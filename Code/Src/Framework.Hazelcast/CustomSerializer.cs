using Hazelcast.Serialization;

namespace Framework.Hazelcast;

public class CustomSerializer<T> : IByteArraySerializer<T>
{
    public void Dispose() { }

    public int TypeId => 1;
    public T Read(byte[] buffer)
    {
        return buffer.ByteDeserializer<T>();
    }

    public byte[] Write(T obj)
    {
        return obj.ByteSerializer<T>();
    }
}