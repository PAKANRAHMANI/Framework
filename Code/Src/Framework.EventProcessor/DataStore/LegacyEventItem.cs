using MongoDB.Bson;

namespace Framework.EventProcessor.DataStore;

public class LegacyEventItem
{
    public ObjectId Id { get; set; }
    public Guid EventId { get; set; }
    public string EventType { get; set; }
    public string Body { get; set; }
    public string AggregateName { get; set; }
    public DateTime PublishDateTime { get; set; }
    public bool IsUsed { get; set; }
}