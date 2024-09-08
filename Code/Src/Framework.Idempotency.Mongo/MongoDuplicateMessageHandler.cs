using Framework.Core.Logging;
using Framework.Idempotency.Mongo.Models;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public class MongoDuplicateMessageHandler(IMongoDatabase database, ILogger logger)
        : IDuplicateMessageHandler
    {
        private readonly IMongoCollection<ProcessedMessage> _processedMessagesCollection = database.GetCollection<ProcessedMessage>(nameof(ProcessedMessage).Pluralize());

        public async Task<bool> HasMessageBeenProcessedBefore(string eventId)
        {
            var filter = Builders<ProcessedMessage>.Filter.Eq(pm => pm.EventId, eventId);

            return await _processedMessagesCollection.CountDocumentsAsync(filter) > 0;
        }

        public async Task MarkMessageAsProcessed(string eventId)
        {
            try
            {
                var processedMessage = new ProcessedMessage
                {
                    EventId = eventId,
                    Id = ObjectId.GenerateNewId(),
                    ReceivedDate = DateTime.UtcNow
                };

                await _processedMessagesCollection.InsertOneAsync(processedMessage);
            }
            catch (Exception e)
            {
                logger.WriteException(e);
            }
        }

        //public async Task<bool> HasMessageBeenProcessedBefore(Guid eventId)
        //{
        //    var messageInboxCollection = database.GetCollection<BsonDocument>(mongoConfiguration.CollectionName);

        //    var filter = Builders<BsonDocument>.Filter.Eq(mongoConfiguration.FieldName, eventId);

        //    var count = await messageInboxCollection.Find(filter).CountDocumentsAsync();

        //    return count > 0;
        //}

        //public async Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate)
        //{
        //    var messageInboxCollection = database.GetCollection<BsonDocument>(mongoConfiguration.CollectionName);

        //    using (var session = await database.Client.StartSessionAsync())
        //    {
        //        try
        //        {
        //            await messageInboxCollection.InsertOneAsync(new BsonDocument
        //            {
        //                { "_id" , ObjectId.GenerateNewId()},
        //                {mongoConfiguration.FieldName,eventId},
        //                {mongoConfiguration.ReceivedDate,receivedDate}
        //            });
        //        }
        //        catch
        //        {
        //            await session.AbortTransactionAsync();
        //            throw;
        //        }
        //        await session.CommitTransactionAsync();
        //    }
        //}
    }
}