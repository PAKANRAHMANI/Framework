using MongoDB.Bson;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public class MongoDuplicateMessageHandler : IDuplicateMessageHandler
    {
        private readonly MongoConfiguration _mongoConfiguration;
        private readonly IMongoDatabase _database;

        public MongoDuplicateMessageHandler(MongoConfiguration mongoConfiguration, IMongoDatabase database)
        {
            _mongoConfiguration = mongoConfiguration;
            _database = database;
        }
        public async Task<bool> HasMessageBeenProcessedBefore(Guid eventId)
        {
            var messageInboxCollection = _database.GetCollection<BsonDocument>(_mongoConfiguration.CollectionName);

            var filter = Builders<BsonDocument>.Filter.Eq(_mongoConfiguration.FieldName, eventId);

            var count = await messageInboxCollection.Find(filter).CountDocumentsAsync();

            return count > 0;
        }

        public async Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate)
        {
            var messageInboxCollection = _database.GetCollection<BsonDocument>(_mongoConfiguration.CollectionName);

            using (var session = await _database.Client.StartSessionAsync())
            {
                try
                {
                    await messageInboxCollection.InsertOneAsync(new BsonDocument
                    {
                        { "_id" , ObjectId.GenerateNewId()},
                        {_mongoConfiguration.FieldName,eventId},
                        {_mongoConfiguration.ReceivedDate,receivedDate}
                    });
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
                await session.CommitTransactionAsync();
            }

        }
    }
}