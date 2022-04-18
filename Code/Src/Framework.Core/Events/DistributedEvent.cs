using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Events
{
    public class DistributedEvent : IDomainEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime PublishDateTime { get; protected set; }
        public bool IsUsed { get; protected set; }
        public string AggregateName { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected DistributedEvent()
        {
            this.EventId = Guid.NewGuid();
            this.PublishDateTime = DateTime.UtcNow;
            this.IsUsed = false;
            this.AggregateName = string.Empty;
            this.AggregateId = Guid.Empty;
        }
    }
}
