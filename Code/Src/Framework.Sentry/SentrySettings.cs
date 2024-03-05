namespace Framework.Sentry;

public class SentrySettings
{
    public string Environment { get; set; }
    public string Dsn { get; set; }
    public bool CaptureBySentry { get; set; }
}