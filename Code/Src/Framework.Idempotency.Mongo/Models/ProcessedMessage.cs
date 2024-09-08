using MongoDB.Bson;

namespace Framework.Idempotency.Mongo.Models;

public class ProcessedMessage
{
    public ObjectId Id { get; set; }
    public string EventId { get; set; }
    public DateTime ReceivedDate { get; set; }
}