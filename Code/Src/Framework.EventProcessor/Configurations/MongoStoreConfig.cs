namespace Framework.EventProcessor.Configurations
{
    public class MongoStoreConfig
    {
        public string CursorCollectionName { get; set; }
        public string EventsCollectionName { get; set; }
        public int PullingInterval { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool IsUsedCursor { get; set; }
    }
}
