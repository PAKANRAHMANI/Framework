namespace Framework.EventProcessor.Configurations
{
    public class SqlStoreConfig
    {
        public string CursorTable { get; set; }
        public string EventTable { get; set; }
        public int PullingInterval { get; set; }
        public string ConnectionString { get; set; }
    }
}
