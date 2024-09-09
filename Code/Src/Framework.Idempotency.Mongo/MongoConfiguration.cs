namespace Framework.Idempotency.Mongo
{
    public class MongoConfiguration
    {
        public string Connection { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string FieldName { get; set; }
        public string ReceivedDate { get; set; }
    }
}
