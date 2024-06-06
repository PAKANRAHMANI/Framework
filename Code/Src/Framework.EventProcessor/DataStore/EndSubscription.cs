namespace Framework.EventProcessor.DataStore
{
    internal sealed class EndSubscription(Action stop) : ISubscription
    {
        public void UnSubscribe()
        {
            stop.Invoke();
        }
        public void Dispose()
        {
            UnSubscribe();
        }
    }
}
