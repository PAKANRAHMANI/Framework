using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Domain;

namespace Framework.DataAccess.EF
{
    public abstract class EfRepository<TKey, T> : IRepository<TKey, T> where T : class, IAggregateRoot
    {
        protected readonly FrameworkDbContext DbContext;
        protected readonly IAggregateRootConfigurator Configurator;
        protected SequenceHelper Sequence { get; private set; }
        protected EfRepository(FrameworkDbContext dbContext, IAggregateRootConfigurator configurator)
        {
            this.DbContext = dbContext;
            this.Configurator = configurator;
            this.Sequence = new SequenceHelper(dbContext);
        }

        public abstract Task<TKey> GetNextId();

        public virtual async Task Create(T aggregate)
        {
            await DbContext.Set<T>().AddAsync(aggregate);
        }

        public Task Remove(T aggregate)
        {
            DbContext.Set<T>().Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<T> Get(TKey key)
        {
            var aggregate = await DbContext.Set<T>().FindAsync(key);

            return Configurator.Config(aggregate);
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            var aggregate = await DbContext.Set<T>().FindAsync(predicate);

            return Configurator.Config(aggregate);
        }
    }
}
