using System.Threading.Tasks;
using Framework.Domain;

namespace Framework.DataAccess.EF
{
    public abstract class EfRepository<TKey, T> : IRepository<TKey, T> where T : class, IAggregateRoot
    {
        protected readonly FrameworkDbContext DbContext;
        protected SequenceHelper Sequence { get; private set; }
        protected EfRepository(FrameworkDbContext dbContext)
        {
            this.DbContext = dbContext;
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
            return await DbContext.Set<T>().FindAsync(key);
        }
    }
}
