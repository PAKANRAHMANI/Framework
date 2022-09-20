using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using NHibernate;
using Framework.Config;
using Framework.Core;

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
            dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
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
