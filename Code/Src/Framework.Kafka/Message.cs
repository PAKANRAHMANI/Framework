﻿using System.Text;
using System.Text.Json;

namespace Framework.Kafka
{
    public class Message<T>
    {
        public MetaData MetaData { get; set; }

        public T Data { get; set; }

        public string SerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(SerializeObject());
        }
    }
}