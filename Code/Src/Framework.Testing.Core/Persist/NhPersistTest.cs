using System;
using System.Reflection;
using System.Transactions;
using NHibernate;
using Framework.DataAccess.NH;

namespace Framework.Testing.Core.Persist
{
    public class NhPersistTest: IDisposable
    {
        protected FakePublisher EventPublisher { get; private set; }
        protected ISession Session { get; }
        private TransactionScope _transactionScope;

        public NhPersistTest(Assembly mappingAssembly, string connectionString)
        {
            _transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            EventPublisher = new FakePublisher();
            Session = new SessionFactoryBuilder()
                .WithMappingsInAssembly(mappingAssembly)
                .SetSessionNameAs("Test")
                .UsingConnectionString(connectionString)
                .Build()
                .OpenSession();
        }
        public void ClearSession()
        {
            Session.Clear();
        }
        public void Dispose()
        {
            Session?.Dispose();
            _transactionScope.Dispose();
        }
    }
}
