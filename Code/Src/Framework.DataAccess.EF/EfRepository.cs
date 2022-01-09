using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Framework.Domain;

namespace Framework.DataAccess.EF
{
    public abstract class EfRepository<TKey, T> : IRepository<TKey, T> where T : class, IAggregateRoot
    {
        protected readonly FrameworkDbContext _dbContext;

        protected EfRepository(FrameworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public abstract Task<TKey> GetNextId();

        public virtual async Task Create(T aggregate)
        {
            await _dbContext.Set<T>().AddAsync(aggregate);
        }

        public Task Remove(T aggregate)
        {
            _dbContext.Set<T>().Remove(aggregate);
            return Task.CompletedTask;
        }

        public async Task<T> Get(TKey key)
        {
            return await _dbContext.Set<T>().FindAsync(key);
        }
    }
}
