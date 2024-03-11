namespace Framework.AspNetCore.Configurations;

public class ExceptionLogConfiguration
{
    private ExceptionLogConfiguration()
    {
        SentryConfiguration = new SentryConfiguration();
    }

    private static ExceptionLogConfiguration? _instance;

    private static readonly object Lock = new();

    public static ExceptionLogConfiguration GetInstance()
    {
        if (_instance != null)
            return _instance;

        lock (Lock)
        {

            if (_instance == null)
            {
                _instance = new ExceptionLogConfiguration();
            }
        }
        return _instance;
    }

    public SentryConfiguration SentryConfiguration { get; set; }
    public bool CaptureBySentry { get; set; }
}