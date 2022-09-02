using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Events
{
    public interface IDistributedEvent:IEvent
    {
        Guid EventId { get; }
        DateTime PublishDateTime { get; }
        public bool IsUsed { get; }
        string AggregateName { get; }
        Guid AggregateId { get; }
    }
}
