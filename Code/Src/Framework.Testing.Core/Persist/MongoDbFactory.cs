using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IO;

namespace Framework.Testing.Core.Persist
{
    public class MongoDbFactory<TDocument> : MongoDbPersistTest<TDocument>
    {
        private static readonly MongoConfiguration Config = GetConfig();
        public IClientSessionHandle ClientSession => Session;

        public IMongoCollection<TDocument> MongoCollection => DbCollection;
        public MongoDbFactory() : base(a =>
        {
            a.ConnectionString = Config.ConnectionString;
            a.DbName = Config.DbName;
            a.IsPluralCollectionName = Config.IsPluralCollectionName;
            a.IsUsingTransaction = Config.IsUsingTransaction;
        })
        {

        }
        protected override object DocumentId { get; set; }
        public void SetDocumentId(object documentId)
        {
            this.DocumentId = documentId;
        }
        private static MongoConfiguration GetConfig()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, $"appsettings.json"), true, true)
                .Build();

            return configuration.GetSection("MongoConfiguration").Get<MongoConfiguration>();
        }
    }
}
