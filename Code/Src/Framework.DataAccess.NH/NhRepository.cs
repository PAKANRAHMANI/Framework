using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Framework.Domain;

namespace Framework.DataAccess.NH
{
    public abstract class NhRepository<TKey, T> : IRepository<TKey, T> where T : IAggregateRoot
    {
        protected ISession Session { get; private set; }

        protected NhRepository(ISession session)
        {
            Session = session;
        }

        public abstract Task<TKey> GetNextId();

        public async Task Create(T aggregate)
        {
            await Session.SaveAsync(aggregate);
        }

        public async Task Remove(T aggregate)
        {
            await Session.DeleteAsync(aggregate);
        }

        public async Task<T> Get(TKey key)
        {
            return await Session.GetAsync<T>(key);
        }
    }
}
