using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Messages
{
    public class MessageContext
    {
        public string CorrelationId { get; set; }
        public IReadOnlyCollection<string> MessageType { get; set; }
        public IMessage Message { get; set; }

        public MessageContext()
        {
            
        }

        public MessageContext(string correlationId, IReadOnlyCollection<string> messageTypes, IMessage message)
        {
            CorrelationId = correlationId;
            MessageType = messageTypes;
            Message = message;
        }
    }
}
