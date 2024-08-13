using Framework.Core.Logging;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

internal sealed class MongoDbFlagEventHandling(IMongoDatabase database, ILogger logger) : IMongoDbEventHandling
{
    public List<EventItem> GetEvents(string collectionName)
    {
        var events = database
            .GetCollection<LegacyEventItem>(collectionName)
            .AsQueryable()
            .Where(eventItem => eventItem.IsUsed == false)
            .OrderBy(item => item.Id)
            .ToList();

        return EventItemMapper.Map(events);
    }

    public void UpdateEvents(string collectionName, EventItem eventItem)
    {
        try
        {
            var eventCollection = database.GetCollection<LegacyEventItem>(collectionName);

            var filter = Builders<LegacyEventItem>.Filter.Eq(item => item.EventId, Guid.Parse(eventItem.EventId));

            var update = Builders<LegacyEventItem>.Update.Set(x => x.IsUsed, true);

            eventCollection.UpdateMany(filter, update);
        }
        catch (Exception exception)
        {
            logger.WriteException(exception);
        }
    }
}