using System;

namespace Framework.Core.Streaming;

public abstract class Stream : IStream
{
    public long Offset { get; set; }
    public string SequenceNumber { get; set; }
    public DateTime ProduceDateTime { get; }

    protected Stream()
    {
        ProduceDateTime = DateTime.UtcNow;
    }
}