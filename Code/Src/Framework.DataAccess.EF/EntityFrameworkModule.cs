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
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
            dependencyRegister.RegisterScoped<IDbContextTransaction,RelationalTransaction>();
            dependencyRegister.RegisterScoped<IUnitOfWork,EfUnitOfWork>();
        }
    }
}
