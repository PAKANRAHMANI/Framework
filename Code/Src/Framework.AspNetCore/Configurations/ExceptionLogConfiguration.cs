namespace Framework.AspNetCore.Configurations
{
    public class ExceptionLogConfiguration
    {
        public SentryConfiguration SentryConfiguration { get; set; }
        public bool CaptureBySentry { get; set; }
    }
}