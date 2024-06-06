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

    public void UpdateEvents(string collectionName, List<EventItem> eventIds = null)
    {
        try
        {
            if (eventIds is null)
                return;

            var eventCollection = database.GetCollection<LegacyEventItem>(collectionName);

            var filter = Builders<LegacyEventItem>.Filter.In(item => item.EventId, eventIds.Select(eventItem => Guid.Parse(eventItem.EventId)));

            var update = Builders<LegacyEventItem>.Update.Set(x => x.IsUsed, true);

            eventCollection.UpdateMany(filter, update);
        }
        catch (Exception exception)
        {
            logger.WriteException(exception);
        }
    }
}