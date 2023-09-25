using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

public class MongoDbFlagEventHandling : IMongoDbEventHandling
{
    private readonly IMongoDatabase _database;

    public MongoDbFlagEventHandling(IMongoDatabase database)
    {
        _database = database;
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
        if (eventIds is null)
            return;

        var eventCollection = _database.GetCollection<LegacyEventItem>(collectionName);

        var filter = Builders<LegacyEventItem>.Filter.In(item => item.EventId, eventIds.Select(eventItem => Guid.Parse(eventItem.EventId)));

        var update = Builders<LegacyEventItem>.Update.Set(x => x.IsUsed, true);

        eventCollection.UpdateMany(filter, update);
    }
}