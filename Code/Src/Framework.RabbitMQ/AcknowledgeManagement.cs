using System.Collections.Generic;
using System.Linq;

namespace Framework.RabbitMQ
{
    public class AcknowledgeManagement : IAcknowledgeManagement
    {
        private readonly List<object> _handlers = new List<object>();

        public void Subscribe(IAcknowledgeHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Publish(AcknowledgeReceived acknowledge)
        {
            var handlers = _handlers.OfType<IAcknowledgeHandler>().ToList();

            handlers.ForEach(handler =>
            {
                handler.Handle(acknowledge);
            });
        }
    }
}