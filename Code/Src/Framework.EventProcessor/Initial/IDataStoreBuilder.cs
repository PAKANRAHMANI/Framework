using Framework.EventProcessor.Configurations;

namespace Framework.EventProcessor.Initial
{
    internal interface IDataStoreBuilder
    {
        IEventLookup ReadFromSqlServer(Action<SqlStoreConfig> config);

        IEventLookup ReadFromMongoDb(Action<MongoStoreConfig> config);

    }
}
