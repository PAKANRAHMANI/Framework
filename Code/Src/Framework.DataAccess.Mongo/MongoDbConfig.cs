namespace Framework.DataAccess.Mongo
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = "mongodb://127.0.0.1:27017";
        public bool UseUrl { get; set; } = true;
        public MongoClientConfiguration ClientSettings { get; set; }
        public bool UseTransaction { get; set; }
        public string DatabaseName { get; set; }
        public string SequenceCollectionName { get; set; }
    }
}