namespace Framework.EventProcessor.DataStore;
internal sealed class EventItem
{
    public long Id { get; set; }
    public string EventId { get; set; }
    public string EventType { get; set; }
    public string Body { get; set; }
    public string AggregateType { get; set; }
    public string AggregateName { get; set; }
    public DateTime PublishDateTime { get; set; }
    public bool IsUsed { get; set; }
}