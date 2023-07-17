using System;

namespace Framework.Core.Streaming
{
    public interface IStream
    {
        long Offset { get; set; }
        string SequenceNumber { get; set; }
        DateTime ProduceDateTime { get; }
    }
}
