using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

internal sealed class MongoDbCursorEventHandling(IMongoDatabase database, MongoStoreConfig mongoStoreConfig, ILogger logger) : IMongoDbEventHandling
{
    public List<EventItem> GetEvents(string collectionName)
    {
        try
        {
            var position = GetCursorPosition();

            logger.Write("Read Cursor Position From Cursor Collection", LogLevel.Information);

            var events= database
                .GetCollection<EventItem>(collectionName)
                .AsQueryable()
                .Where(eventItem => eventItem.Id > position)
                .OrderBy(item => item.Id)
                .ToList();

            logger.Write($"Read Events From {collectionName} Collection",LogLevel.Information);

            return events;
        }
        catch (Exception e)
        {
            logger.WriteException(e);
        }

        return null;
    }

    public void UpdateEvents(string collectionName, List<EventItem> eventIds = null)
    {
        try
        {
            var cursorCollection = database.GetCollection<Cursor>(collectionName);

            var cursor = cursorCollection.AsQueryable().FirstOrDefault();

            if (eventIds is null)
                return;

            if (cursor is null)
                return;

            var @event = eventIds.LastOrDefault();

            if (@event is not null)
            {
                cursor.Position = @event.Id;
            }

            var filter = Builders<Cursor>.Filter.Eq(s => s.Id, cursor?.Id);

            cursorCollection.ReplaceOneAsync(filter, cursor);
        }
        catch (Exception exception)
        {
            logger.WriteException(exception);
        }
    }

    private long GetCursorPosition()
    {
        var cursor = database.GetCollection<Cursor>(mongoStoreConfig.CursorCollectionName).AsQueryable().FirstOrDefault();

        return cursor?.Position ?? 0;
    }
}