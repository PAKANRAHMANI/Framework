using System.Threading.Tasks;
using Framework.Core;
using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly IClientSessionHandle _transaction;

        public MongoUnitOfWork(IMongoContext context)
        {
            this._transaction = context.GetSession();
        }
        public async Task Begin()
        {
            this._transaction.StartTransaction();
            await Task.CompletedTask;
        }

        public async Task Commit()
        {
            await _transaction.CommitTransactionAsync();
        }

        public async Task RollBack()
        {
            await this._transaction.AbortTransactionAsync();
        }
    }
}
