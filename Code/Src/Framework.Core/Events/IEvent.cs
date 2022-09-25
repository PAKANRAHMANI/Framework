using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Events
{
    public interface IEvent
    {
        Guid EventId { get; }
        DateTime PublishDateTime { get; }
    }
}
