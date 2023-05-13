namespace Framework.DataAccess.Mongo
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = "mongodb://127.0.0.1:27017";
        public string DatabaseName { get; set; }
        public bool IsDecorateTransactionForCommands { get; set; }
        public bool IsDecorateTransactionForRequests { get; set; }
    }
}