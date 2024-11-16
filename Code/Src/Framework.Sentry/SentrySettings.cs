namespace Framework.Sentry;

public class SentrySettings
{
    public string Environment { get; set; }
    public string Dsn { get; set; }
    public bool CaptureBySentry { get; set; }
    public float? SampleRate { get; set; } = 1f;
}