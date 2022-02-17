using System;

namespace Framework.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
        DateTime PublishDateTime { get; }
    }
}
