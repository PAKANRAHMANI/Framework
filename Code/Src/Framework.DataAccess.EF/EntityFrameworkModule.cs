using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Framework.Application.Contracts;
using Framework.Config;
using Framework.Core;

namespace Framework.DataAccess.EF
{
    public class EntityFrameworkModule : IFrameworkModule
    {
        private readonly bool _isUsingRequestHandler;

        public EntityFrameworkModule(bool isUsingRequestHandler = false)
        {
            _isUsingRequestHandler = isUsingRequestHandler;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            if (_isUsingRequestHandler)
                dependencyRegister.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionalRequestHandlerDecorator<,>));
            else
                dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));

            dependencyRegister.RegisterScoped<IDbContextTransaction, RelationalTransaction>();
            dependencyRegister.RegisterScoped<IUnitOfWork, EfUnitOfWork>();
        }
    }
}
