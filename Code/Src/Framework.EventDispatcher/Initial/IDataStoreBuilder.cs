using Framework.EventProcessor.DataStore.MongoDB;
using Framework.EventProcessor.DataStore.Sql;

namespace Framework.EventProcessor.Initial
{
    public interface IDataStoreBuilder
    {
        IEventLookup ReadFromSqlServer(Action<SqlStoreConfig> config);

        IEventLookup ReadFromMongoDb(Action<MongoStoreConfig> config);

    }
}
