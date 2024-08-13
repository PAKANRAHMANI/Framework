using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB;

public class MongoUpdateCursorPosition(MongoStoreConfig mongoStoreConfig, IMongoDatabase database, ILogger logger)
    : IUpdateCursorPosition
{
    private readonly IMongoDbEventHandling _mongoDbEvent = MongoEventFactory.Create(mongoStoreConfig, database, logger);

    public void MoveCursorPosition(EventItem eventItem)
    {
        var collectionName = mongoStoreConfig.IsUsedCursor
            ? mongoStoreConfig.CursorCollectionName
            : mongoStoreConfig.EventsCollectionName;

        _mongoDbEvent.UpdateEvents(collectionName, eventItem);

        logger.Write($"cursor moved to : {eventItem.Id}", LogLevel.Information);
    }
}