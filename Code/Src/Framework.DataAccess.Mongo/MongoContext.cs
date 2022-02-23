using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public class MongoContext : IMongoContext
    {
        private readonly IClientSessionHandle _session;
        public MongoContext(IMongoDatabase database)
        {
            this._session = database.Client.StartSession();
        }
        public IClientSessionHandle GetSession()
        {
            return _session;
        }
    }
}