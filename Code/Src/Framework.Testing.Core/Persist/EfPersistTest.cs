using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Framework.DataAccess.EF;
using Framework.Testing.Core.Fakes;
using Microsoft.EntityFrameworkCore;

namespace Framework.Testing.Core.Persist
{
    public abstract class EfPersistTest<TDbContext> : IDisposable where TDbContext : FrameworkDbContext
    {
        public readonly EfUnitOfWork EfUnitOfWork;
        public TDbContext DbContext;
        public FakeConfigurator Configurator { get; private set; }
        protected EfPersistTest(string connectionString)
        {
            this.Configurator = new FakeConfigurator();
            var option = new DbContextOptionsBuilder().UseSqlServer(connectionString);
            this.DbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), true, option.Options);
            this.EfUnitOfWork = new EfUnitOfWork(this.DbContext);
        }

        public void Dispose()
        {
            this.DbContext?.Dispose();
        }
    }
}
