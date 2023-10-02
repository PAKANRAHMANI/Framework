using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Framework.DataAccess.Mongo
{
    public class Sequence
    {
        [BsonId]
        public ObjectId _Id { get; set; }

        public string Name { get; set; }

        public long Value { get; set; }
    }
}
