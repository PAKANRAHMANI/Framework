using System;
using System.Reflection;
using System.Transactions;
using NHibernate;
using Framework.DataAccess.NH;
using Framework.Testing.Core.Fakes;

namespace Framework.Testing.Core.Persist
{
    public class NhPersistTest: IDisposable
    {
        public FakeConfigurator Configurator { get; private set; }
        protected ISession Session { get; }
        private readonly TransactionScope _transactionScope;

        public NhPersistTest(Assembly mappingAssembly, string connectionString)
        {
            this._transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            this.Configurator = new FakeConfigurator();
            this.Session = new SessionFactoryBuilder()
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
