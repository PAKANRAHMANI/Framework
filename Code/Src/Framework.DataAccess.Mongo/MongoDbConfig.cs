namespace Framework.DataAccess.Mongo
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool IsDecorateTransactionForCommands { get; set; }
    }
}