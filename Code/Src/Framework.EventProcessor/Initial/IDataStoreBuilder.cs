using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Initial
{
    public interface IDataStoreBuilder
    {
        IEventLookup ReadFromSqlServer(Action<SqlStoreConfig> config);

        IEventLookup ReadFromMongoDb(Action<MongoStoreConfig> config);

    }
}
