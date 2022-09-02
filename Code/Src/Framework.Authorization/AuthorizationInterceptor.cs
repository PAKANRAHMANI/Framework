using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Castle.DynamicProxy;
using Framework.Core;

namespace Framework.Authorization
{
    public class AuthorizationInterceptor : IInterceptor
    {
        private readonly ICurrentUser _currentUser;
        private readonly IAuthorizationProvider _authorizationProvider;

        public AuthorizationInterceptor(ICurrentUser currentUser, IAuthorizationProvider authorizationProvider)
        {
            _currentUser = currentUser;
            _authorizationProvider = authorizationProvider;
        }
        public void Intercept(IInvocation invocation)
        {
            var allAttributes = invocation.Method.GetCustomAttributes().ToList();
            if (IsIgnorePermission(allAttributes))
                invocation.Proceed();
            else
            {
                if (UserIsNotLoggedIn())
                    throw new UnauthorizedAccessException("The User Is Not Logged In");
                if (PermissionAttributeNotFound(allAttributes))
                    throw new UnauthorizedAccessException("The Method Does Not have Permission Attribute");
                var attribute = allAttributes.OfType<HasPermissionAttribute>().First();
                if (CurrentUserHasPermission(attribute.Permission))
                    invocation.Proceed();
                else
                    throw new UnauthorizedAccessException("The Method Does Not have Permission");
            }
        }
        private bool IsIgnorePermission(List<Attribute> allAttributes)
        {
            return allAttributes.OfType<IgnorePermissionAttribute>().Any();
        }
        private bool UserIsNotLoggedIn()
        {
            return !this._currentUser.Get().Identity.IsAuthenticated;
        }
        private bool PermissionAttributeNotFound(List<Attribute> allAttributes)
        {
            return !allAttributes.OfType<HasPermissionAttribute>().Any();
        }
        private bool CurrentUserHasPermission(Permission permission)
        {
            return this._currentUser.Get().Identity is ClaimsIdentity user && _authorizationProvider.HasPermission(permission, user);
        }
    }
}
