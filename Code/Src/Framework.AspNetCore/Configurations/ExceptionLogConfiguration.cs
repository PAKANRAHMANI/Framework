namespace Framework.AspNetCore.Configurations
{
    public class ExceptionLogConfiguration
    {
        public ExceptionLogConfiguration()
        {
            this.SentryConfiguration = new SentryConfiguration();
        }
        public SentryConfiguration SentryConfiguration { get; set; }
        public bool CaptureBySentry { get; set; }
    }
}