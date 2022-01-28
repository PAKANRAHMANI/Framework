using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Kafka
{
    public interface IMessageSender<T>
    {
        Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default);
    }

    public class MetaData
    {
        public string MessageId { get; set; }

        public string MessageVersion { get; set; }

        public string CorrelationId { get; set; }

        public DateTimeOffset? CreationDateTime { get; set; }

        public DateTimeOffset? EnqueuedDateTime { get; set; }

        public string AccessToken { get; set; }
    }
}