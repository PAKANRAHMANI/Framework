using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class MongoEventFactory
    {
        public static IMongoDbEventHandling Create(MongoStoreConfig mongoStoreConfig, IMongoDatabase database)
        {
            if (mongoStoreConfig.IsUsedCursor)
                return new MongoDbCursorEventHandling(database, mongoStoreConfig);

            return new MongoDbFlagEventHandling(database);
        }
    }
}
