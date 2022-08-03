using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Framework.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace Framework.Testing.Core.Persist
{
    public abstract class EfPersistTest : IDisposable
    {
        public readonly EfUnitOfWork EfUnitOfWork;
        public FrameworkDbContext DbContext;
        public FakePublisher Publisher { get; private set; }
        protected EfPersistTest(string connectionString)
        {
            this.Publisher = new FakePublisher();
            var option = new DbContextOptionsBuilder().UseSqlServer(connectionString);
            this.DbContext = new TestDbContext(option.Options);
            this.EfUnitOfWork = new EfUnitOfWork(this.DbContext);
        }

        public void Dispose()
        {
            this.DbContext?.Dispose();
        }
    }
}
