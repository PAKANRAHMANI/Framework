namespace Framework.EventProcessor.DataStore;

public interface ISubscription : IDisposable
{
    void UnSubscribe();
}