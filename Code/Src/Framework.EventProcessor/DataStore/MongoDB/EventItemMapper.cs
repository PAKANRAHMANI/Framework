namespace Framework.EventProcessor.DataStore.MongoDB
{
    public static class EventItemMapper
    {
        public static List<EventItem> Map(List<LegacyEventItem> items)
        {
            return items.Select(Map).ToList();
        }
        public static EventItem Map(LegacyEventItem item)
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
