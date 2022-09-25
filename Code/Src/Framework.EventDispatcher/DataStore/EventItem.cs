namespace Framework.EventProcessor.DataStore;
public class EventItem
{
    public long Id { get; set; }
    public Guid EventId { get; set; }
    public string EventType { get; set; }
    public string Body { get; set; }
    public Type AggregateType { get; set; }
    public DateTime PublishDateTime { get; set; }
}