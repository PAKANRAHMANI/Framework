namespace Framework.EventProcessor.DataStore;

internal interface ISubscription : IDisposable
{
    void UnSubscribe();
}