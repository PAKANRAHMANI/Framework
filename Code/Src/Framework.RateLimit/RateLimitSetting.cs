namespace Framework.RateLimit
{
    public class RateLimitSetting
    {
        public int PermitLimit { get; set; }
        public int Window { get; set; }
        public int QueueLimit { get; set; }
        public int StatusCode { get; set; }
        public int QueueProcessingOrder { get; set; } = (int)System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        public bool AutoReplenishment { get; set; } = true;
    }
}
