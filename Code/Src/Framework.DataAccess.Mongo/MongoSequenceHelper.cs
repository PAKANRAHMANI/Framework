using MongoDB.Driver;

namespace Framework.DataAccess.Mongo;

public class MongoSequenceHelper
{
    private readonly IMongoDatabase _database;

    public MongoSequenceHelper(IMongoDatabase database)
    {
        _database = database;
    }
    public long GetNextId(string sequenceName)
    {
        var sequenceCollection = _database.GetCollection<Sequence>(sequenceName);

        var filter = Builders<Sequence>.Filter.Eq(a => a.Name, sequenceName);

        var update = Builders<Sequence>.Update.Inc(a => a.Value, 1);

        var sequence = sequenceCollection.FindOneAndUpdate(filter, update);

        return sequence.Value;
    }
}