using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Framework.Core;

namespace Framework.DataAccess.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly FrameworkDbContext _dbContext;
        private IDbContextTransaction _transaction;
        public EfUnitOfWork(FrameworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Begin()
        {
            this._transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await Task.CompletedTask;
        }

        public async Task Commit()
        {
            await _dbContext.Database.CurrentTransaction.CommitAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            await this._transaction.RollbackAsync();
        }
    }
}
