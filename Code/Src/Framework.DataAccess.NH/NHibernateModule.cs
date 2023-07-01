using Framework.Config;
using Framework.Core;
using NHibernate;
using System.Data.SqlClient;

namespace Framework.DataAccess.NH
{
    public class NHibernateModule : IFrameworkModule
    {
        private readonly SessionFactoryBuilder _sessionFactory;

        public NHibernateModule(SessionFactoryBuilder sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped(CreateSession, a => a.Close());
            dependencyRegister.RegisterScoped<IUnitOfWork, NhUnitOfWork>();
        }
        private ISession CreateSession()
        {
            var builder = _sessionFactory.Build().WithOptions();
            var connection = new SqlConnection(_sessionFactory.ConnectionString);
            connection.Open();
            return builder.Connection(connection).OpenSession();
        }
    }
}
