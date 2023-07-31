using System;

namespace Framework.Messages
{
    public interface IMessage
    {
        Type MessageType { get; }
        string MessageTypeString { get; }
        Guid MessageId { get; }
        DateTime PublishDateTime { get; }
        public string Body { get; set; }
    }
}
