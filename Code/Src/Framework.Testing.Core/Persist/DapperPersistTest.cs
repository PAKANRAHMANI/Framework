using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Framework.Testing.Core.Persist
{
    public class DapperPersistTest : IDisposable
    {
        private readonly TransactionScope _transactionScope;
        protected IDbConnection DbConnection { get; }
        public DapperPersistTest(string connectionString)
        {
            this._transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            this.DbConnection = new SqlConnection(connectionString);
            this.DbConnection.Open();
        }
        public void Dispose()
        {
            this.DbConnection.Dispose();
            this._transactionScope.Dispose();
            this.DbConnection.Close();
        }
    }
}
