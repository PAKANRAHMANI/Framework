namespace Framework.Sentry
{
    /// <summary>
    /// Capture Exception by sentry
    /// </summary>
    public class SentryService : ISentryService
    {
        private readonly SentrySettings _sentrySettings;

        public SentryService(SentrySettings sentrySettings)
        {
            _sentrySettings = sentrySettings;
        }
        /// <summary>
        /// capture exception by sentry
        /// </summary>
        /// <param name="exception">your exception</param>
        public void CaptureException(Exception exception)
        {
            if (_sentrySettings.CaptureBySentry == false)
                return;

            using (SentrySdk.Init(option =>
                   {
                       option.Environment = _sentrySettings.Environment;
                       option.Dsn = _sentrySettings.Dsn;
                       option.SampleRate = _sentrySettings.SampleRate;
                   }))
            {
                SentrySdk.CaptureException(exception);
            }
        }
    }
}
