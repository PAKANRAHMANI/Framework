using System;
using System.Reflection;
using Framework.DataAccess.EF;
using Framework.Testing.Core.Fakes;
using Microsoft.EntityFrameworkCore;

namespace Framework.Testing.Core.Persist
{
    public abstract class EfPersistTestDbContext<TDbContext> : FrameworkDbContext, IDisposable where TDbContext : FrameworkDbContext
    {
        private readonly Assembly _mappingAssembly;
        public readonly EfUnitOfWork EfUnitOfWork;
        public TDbContext DbContext;
        public FakeConfigurator Configurator { get; private set; }
        protected EfPersistTestDbContext(string connectionString, Assembly mappingAssembly)
        {
            this._mappingAssembly = mappingAssembly;

            this.Configurator = new FakeConfigurator();

            var option = new DbContextOptionsBuilder().UseSqlServer(connectionString);

            this.DbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), option.Options);

            this.EfUnitOfWork = new EfUnitOfWork(this.DbContext);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(_mappingAssembly);

            base.OnModelCreating(modelBuilder);
        }
        public void Dispose()
        {
            this.DbContext?.Dispose();
        }
    }
}
