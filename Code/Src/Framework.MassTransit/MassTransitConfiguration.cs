namespace Framework.MassTransit
{
    public class MassTransitConfiguration
    {
        public string Connection { get; set; }
        public string QueueName { get; set; }
        public bool ConfigureConsumeTopology { get; set; }
        public RetryConfiguration RetryConfiguration { get; set; }
        public int PrefetchCount { get; set; }
        public string ExchangeType { get; set; }
        public byte? Priority { get; set; }
    }
}