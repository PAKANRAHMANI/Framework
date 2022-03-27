using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Framework.MassTransit
{
    public class MassTransitMessage
    {
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string ConversationId { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public IList<string> MessageType { get; set; }
        public JToken Message { get; set; }
        public DateTime SentTime { get; set; }
        public MassTransitHost Host { get; set; }
    }
}