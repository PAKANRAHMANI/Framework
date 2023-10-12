using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public class MongoDuplicateMessageHandler : IDuplicateMessageHandler
    {
        private readonly MongoConfiguration _mongoConfiguration;
        private readonly IMongoCollection<BsonDocument> _messageInboxCollection;

        public MongoDuplicateMessageHandler(MongoConfiguration mongoConfiguration, IMongoDatabase database)
        {
            _mongoConfiguration = mongoConfiguration;
            _messageInboxCollection = database.GetCollection<BsonDocument>(_mongoConfiguration.CollectionName);
        }
        public async Task<bool> HasMessageBeenProcessedBefore(Guid eventId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(_mongoConfiguration.FieldName, eventId);

            var count = await _messageInboxCollection.Find(filter).CountDocumentsAsync();

            return count > 0;
        }

        public async Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate)
        {
            await _messageInboxCollection.InsertOneAsync(new BsonDocument
            {
                { "_id" , ObjectId.GenerateNewId()},
                {_mongoConfiguration.FieldName,eventId},
                {_mongoConfiguration.ReceivedDate,receivedDate}
            });
        }
    }
}