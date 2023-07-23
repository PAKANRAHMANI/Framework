using MongoDB.Driver;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    //TODO:handle cursor null in queries
    public static class MongoQueries
    {
        public static long GetCursorPosition(this IMongoDatabase database, string documentName)
        {
            var cursor = database.GetCollection<Cursor>(documentName).AsQueryable().FirstOrDefault();

            return cursor.Position;
        }
        public static void MoveCursorPosition(this IMongoDatabase database, long position, string documentName)
        {
            var cursorCollection = database.GetCollection<Cursor>(documentName);

            var cursor = cursorCollection.AsQueryable().FirstOrDefault();

            cursor.Position = position;

            var filter = Builders<Cursor>.Filter.Eq(s => s.Id, cursor.Id);

            cursorCollection.ReplaceOneAsync(filter, cursor);
        }

        public static List<EventItem> GetEventsFromPosition(this IMongoDatabase database, long position, string documentName)
        {
            return database
                .GetCollection<EventItem>(documentName)
                .AsQueryable()
                .Where(eventItem => eventItem.Id > position)
                .OrderBy(item => item.Id)
                .ToList();
        }
    }
}
