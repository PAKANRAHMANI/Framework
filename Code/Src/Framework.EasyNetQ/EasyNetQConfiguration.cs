using EasyNetQ;
using EasyNetQ.Topology;

namespace Framework.EasyNetQ
{
    public class EasyNetQConfiguration
    {
        public Exchange Exchange { get; set; }
        public string RoutingKey { get; set; }
        public MessageProperties MessageProperties { get; set; }
    }
}