using Framework.Config;
using Framework.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.DataAccess.EF
{
    public class EntityFrameworkModule : IFrameworkModule
    {
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<IDbContextTransaction, RelationalTransaction>();
            dependencyRegister.RegisterScoped<IUnitOfWork, EfUnitOfWork>();
        }
    }
}
