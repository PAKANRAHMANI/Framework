using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate;
using Framework.Core;

namespace Framework.DataAccess.NH
{
    public class NhUnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;

        public NhUnitOfWork(ISession session)
        {
            this._session = session;
        }
        public Task Begin()
        {
            this._session.Transaction.Begin(IsolationLevel.ReadCommitted);
            return Task.CompletedTask;
        }

        public async Task Commit()
        {
            await this._session.Transaction.CommitAsync();
        }

        public async Task RollBack()
        {
            await this._session.Transaction.RollbackAsync();
        }
    }
}
