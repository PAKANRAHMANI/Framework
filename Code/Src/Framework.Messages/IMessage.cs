using System;

namespace Framework.Messages
{
    public interface IMessage
    {
        Type MessageType { get; }
        Guid MessageId { get; }
        DateTime PublishDateTime { get; }
    }
}
