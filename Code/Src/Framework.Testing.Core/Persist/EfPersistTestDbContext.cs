using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Framework.DataAccess.EF;
using Framework.Testing.Core.Fakes;
using Microsoft.EntityFrameworkCore;

namespace Framework.Testing.Core.Persist
{
    public abstract class EfPersistTestDbContext<TDbContext> : FrameworkDbContext, IDisposable where TDbContext : FrameworkDbContext
    {
        private readonly Assembly _mappingAssembly;
        protected readonly TransactionScope TransactionScope;
        protected TDbContext DbContext;
        protected FakeConfigurator Configurator { get; private set; }
        protected EfPersistTestDbContext(string connectionString, Assembly mappingAssembly)
        {
            this.TransactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

            this._mappingAssembly = mappingAssembly;

            this.Configurator = new FakeConfigurator();

            var option = new DbContextOptionsBuilder().UseSqlServer(connectionString);

            this.DbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), option.Options);

        }


        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(_mappingAssembly);

            base.OnModelCreating(modelBuilder);
        }
        public void Dispose()
        {
            this.DbContext?.Dispose();
            this.TransactionScope.Dispose();
        }
    }
}
