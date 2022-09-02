using System;
using Castle.DynamicProxy;
using Framework.Config;

namespace Framework.Authorization
{
    public class AuthorizationModule : IFrameworkModule
    {
        private readonly Type _provider;

        public AuthorizationModule(Type provider)
        {
            _provider = provider;
        }
        public void Register(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.RegisterScoped<IAuthorizationProvider>(_provider);
            dependencyRegister.RegisterScoped<IInterceptor, AuthorizationInterceptor>();
        }
    }
}
