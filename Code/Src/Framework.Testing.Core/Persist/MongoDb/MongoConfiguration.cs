namespace Framework.Testing.Core.Persist.MongoDb
{
    public class MongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
        public bool IsPluralCollectionName { get; set; } = true;
        public bool IsUsingTransaction { get; set; }
    }
}
