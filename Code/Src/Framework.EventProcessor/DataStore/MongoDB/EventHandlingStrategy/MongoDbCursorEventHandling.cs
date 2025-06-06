﻿using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy;

internal sealed class MongoDbCursorEventHandling(IMongoDatabase database, MongoStoreConfig mongoStoreConfig, ILogger logger) : IMongoDbEventHandling
{
    public List<EventItem> GetEvents(string collectionName)
    {
        try
        {
            var position = GetCursorPosition();

            logger.Write("Read Cursor Position From Cursor Collection", LogLevel.Debug);

            var events = database
                .GetCollection<EventItem>(collectionName)
                .AsQueryable()
                .Where(eventItem => eventItem.Id > position)
                .OrderBy(item => item.Id)
                .ToList();

            logger.Write($"Read Events From {collectionName} Collection", LogLevel.Debug);

            return events;
        }
        catch (Exception e)
        {
            logger.WriteException(e);
        }

        return null;
    }

    public void UpdateEvents(string collectionName, EventItem eventItem)
    {
        try
        {
            var cursorCollection = database.GetCollection<Cursor>(collectionName);

            var cursor = cursorCollection.AsQueryable().FirstOrDefault();

            if (cursor is null)
                return;

            cursor.Position = eventItem.Id;

            var filter = Builders<Cursor>.Filter.Eq(s => s.Id, cursor?.Id);

            cursorCollection.ReplaceOneAsync(filter, cursor);
        }
        catch (Exception exception)
        {
            logger.WriteException(exception);
        }
    }

    private long GetCursorPosition()
    {
        var cursor = database.GetCollection<Cursor>(mongoStoreConfig.CursorCollectionName).AsQueryable().FirstOrDefault();

        return cursor?.Position ?? 0;
    }
}