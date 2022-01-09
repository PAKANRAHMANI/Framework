using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DataAccess.EF
{
    public class DomainEventStructure
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
        public DateTime PublishDateTime { get; set; }
    }
}
