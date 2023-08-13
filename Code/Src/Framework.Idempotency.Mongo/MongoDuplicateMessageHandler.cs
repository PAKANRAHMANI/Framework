using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public class MongoDuplicateMessageHandler : IDuplicateMessageHandler
    {
        private readonly MongoConfiguration _mongoConfiguration;
        private readonly IMongoCollection<BsonDocument> _ipoMessageInboxCollection;

        public MongoDuplicateMessageHandler(IOptions<MongoConfiguration> mongoConfiguration, IMongoDatabase database)
        {
            _mongoConfiguration = mongoConfiguration.Value;
            _ipoMessageInboxCollection = database.GetCollection<BsonDocument>(_mongoConfiguration.CollectionName);
        }
        public async Task<bool> HasMessageBeenProcessedBefore(Guid eventId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(_mongoConfiguration.FieldName, eventId);

            var count = await _ipoMessageInboxCollection.Find(filter).CountDocumentsAsync();

            return count > 0;
        }

        public async Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate)
        {
            await _ipoMessageInboxCollection.InsertOneAsync(new BsonDocument
            {
                { "_id" , ObjectId.GenerateNewId()},
                {_mongoConfiguration.FieldName,eventId},
                {_mongoConfiguration.ReceivedDate,receivedDate}
            });
        }
    }
}