namespace Framework.EventProcessor.DataStore.MongoDB
{
    internal static class EventItemMapper
    {
        internal static List<EventItem> Map(List<LegacyEventItem> items)
        {
            return items.Select(Map).ToList();
        }
        internal static EventItem Map(LegacyEventItem item)
        {
            return new EventItem()
            {
                EventId = item.EventId.ToString(),
                IsUsed = item.IsUsed,
                PublishDateTime = item.PublishDateTime,
                AggregateName = item.AggregateName,
                Body = item.Body,
                EventType = item.EventType
            };
        }
    }
}
