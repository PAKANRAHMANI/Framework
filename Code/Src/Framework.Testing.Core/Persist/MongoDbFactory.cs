using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

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
            a.IsPluralCollectionName = false;
            a.IsUsingTransaction = true;
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
