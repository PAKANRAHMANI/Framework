using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

public class MongoDbCursorEventHandling : IMongoDbEventHandling
{
    private readonly IMongoDatabase _database;
    private readonly MongoStoreConfig _mongoStoreConfig;
    private readonly ILogger _logger;

    public MongoDbCursorEventHandling(IMongoDatabase database, MongoStoreConfig mongoStoreConfig,ILogger logger)
    {
        _database = database;
        _mongoStoreConfig = mongoStoreConfig;
        _logger = logger;
    }
    public List<EventItem> GetEvents(string collectionName)
    {
        try
        {
            var position = GetCursorPosition();

            _logger.Write("Read Cursor Position From Cursor Collection", LogLevel.Information);

            var events= _database
                .GetCollection<EventItem>(collectionName)
                .AsQueryable()
                .Where(eventItem => eventItem.Id > position)
                .OrderBy(item => item.Id)
                .ToList();

            _logger.Write($"Read Events From {collectionName} Collection",LogLevel.Information);

            return events;
        }
        catch (Exception e)
        {
            _logger.WriteException(e);
        }

        return null;
    }

    public void UpdateEvents(string collectionName, List<EventItem> eventIds = null)
    {
        try
        {
            var cursorCollection = _database.GetCollection<Cursor>(collectionName);

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
            _logger.WriteException(exception);
        }
    }

    private long GetCursorPosition()
    {
        var cursor = _database.GetCollection<Cursor>(_mongoStoreConfig.CursorCollectionName).AsQueryable().FirstOrDefault();

        return cursor?.Position ?? 0;
    }
}