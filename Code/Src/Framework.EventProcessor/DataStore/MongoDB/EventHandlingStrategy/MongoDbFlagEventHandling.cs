using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

public class MongoDbFlagEventHandling : IMongoDbEventHandling
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbFlagEventHandling> _logger;

    public MongoDbFlagEventHandling(IMongoDatabase database, ILogger<MongoDbFlagEventHandling> logger)
    {
        _database = database;
        _logger = logger;
    }

    public List<EventItem> GetEvents(string collectionName)
    {
        var events = _database
            .GetCollection<LegacyEventItem>(collectionName)
            .AsQueryable()
            .Where(eventItem => eventItem.IsUsed == false)
            .OrderBy(item => item.Id)
            .ToList();
        return EventItemMapper.Map(events);
    }

    public void UpdateEvents(string collectionName, List<EventItem> eventIds = null)
    {
        try
        {
            if (eventIds is null)
                return;

            var eventCollection = _database.GetCollection<LegacyEventItem>(collectionName);

            var filter = Builders<LegacyEventItem>.Filter.In(item => item.EventId, eventIds.Select(eventItem => Guid.Parse(eventItem.EventId)));

            var update = Builders<LegacyEventItem>.Update.Set(x => x.IsUsed, true);

            eventCollection.UpdateMany(filter, update);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }
    }
}