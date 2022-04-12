﻿using System;

namespace Framework.Core.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime PublishDateTime { get; protected set; }
        public bool IsUsed { get; protected set; }
        public string AggregateName { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected DomainEvent()
        {
            this.EventId = Guid.NewGuid();
            this.PublishDateTime = DateTime.UtcNow;
            this.IsUsed = false;
            this.AggregateName = string.Empty;
            this.AggregateId = Guid.Empty;
        }
    }
}
