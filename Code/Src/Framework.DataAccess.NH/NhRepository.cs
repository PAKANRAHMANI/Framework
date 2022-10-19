using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Framework.Domain;
using NHibernate.Linq;

namespace Framework.DataAccess.NH
{
    public abstract class NhRepository<TKey, T> : IRepository<TKey, T> where T : IAggregateRoot
    {
        private readonly IAggregateRootConfigurator _configurator;
        protected ISession Session { get; private set; }
        protected SequenceHelper Sequence { get; private set; }

        protected NhRepository(ISession session, IAggregateRootConfigurator configurator)
        {
            this._configurator = configurator;
            this.Session = session;
            this.Sequence = new SequenceHelper(session);
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
            var aggregate = await Session.GetAsync<T>(key);

            return _configurator.Config(aggregate);
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            var aggregate = await Session.Query<T>().Where(predicate).FirstOrDefaultAsync();

            return _configurator.Config(aggregate);
        }
    }
}
