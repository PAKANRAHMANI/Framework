namespace Framework.EventProcessor.DataStore
{
    public class EndSubscription : ISubscription
    {
        private readonly Action _stop;
        public EndSubscription(Action stop)
        {
            _stop = stop;
        }

        public void UnSubscribe()
        {
            _stop.Invoke();
        }
        public void Dispose()
        {
            UnSubscribe();
        }
    }
}
