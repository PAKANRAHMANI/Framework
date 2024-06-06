﻿namespace Framework.EventProcessor.DataStore.MongoDB.EventHandlingStrategy
{
    internal interface IMongoDbEventHandling
    {
        List<EventItem> GetEvents(string collectionName);
        void UpdateEvents(string collectionName, List<EventItem> eventIds = null);
    }
}
