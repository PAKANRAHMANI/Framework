namespace Framework.Sentry;

/// <summary>
/// Capture Exception by sentry
/// </summary>
public interface ISentryService
{
    void CaptureException(Exception exception);
}