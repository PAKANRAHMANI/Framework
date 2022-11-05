using ProtoBuf;

namespace Framework.Hazelcast;

public static class SerializerDeserializerExtensions
{
    public static byte[] ByteSerializer<T>(this T obj)
    {
        using var memoryStream = new MemoryStream();
        Serializer.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }

    public static T ByteDeserializer<T>(this byte[] byteArray)
    {
        using var memoryStream = new MemoryStream(byteArray);
        return Serializer.Deserialize<T>(memoryStream);
    }
    public static CustomSerializer<T> CreateCustomSerializer<T>(T item)
    {
        var xx = Activator.CreateInstance(typeof(CustomSerializer<T>)) as CustomSerializer<T>;
        return xx;
    }
}