namespace Framework.EventProcessor.Configurations
{
    public sealed record SqlStoreConfig
    {
        public string CursorTable { get; set; }
        public string EventTable { get; set; }
        public int PullingInterval { get; set; }
        public string ConnectionString { get; set; }
    }
}
