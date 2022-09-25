namespace Framework.EventProcessor.DataStore;
public class EventItem
{
    public long Id { get; set; }
    public string EventId { get; set; }
    public string EventType { get; set; }
    public string Body { get; set; }
    public string AggregateType { get; set; }
    public DateTime PublishDateTime { get; set; }
}