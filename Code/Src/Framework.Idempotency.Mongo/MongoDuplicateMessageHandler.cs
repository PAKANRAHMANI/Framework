using Framework.Core.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Framework.Idempotency.Mongo
{
    public class MongoDuplicateMessageHandler(IMongoDatabase database, MongoConfiguration mongoConfiguration, ILogger logger)
        : IDuplicateMessageHandler
    {
        public async Task<bool> HasMessageBeenProcessedBefore(string eventId)
        {
            try
            {
                var messageInboxCollection = database.GetCollection<BsonDocument>(mongoConfiguration.CollectionName);

                var filter = Builders<BsonDocument>.Filter.Eq(mongoConfiguration.FieldName, eventId);

                return await messageInboxCollection.CountDocumentsAsync(filter) > 0;
            }
            catch (Exception ex)
            {
                logger.WriteException(ex);

                return false;
            }
        }

        public async Task MarkMessageAsProcessed(string eventId)
        {
            using (var session = await database.Client.StartSessionAsync())
            {
                try
                {
                    var messageInboxCollection = database.GetCollection<BsonDocument>(mongoConfiguration.CollectionName);

                    await messageInboxCollection.InsertOneAsync(new BsonDocument
                    {
                        { "_id" , ObjectId.GenerateNewId()},
                        {mongoConfiguration.FieldName,eventId},
                        {mongoConfiguration.ReceivedDate,DateTime.UtcNow}
                    });
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();

                    logger.WriteException(ex);

                    throw;
                }
                await session.CommitTransactionAsync();
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