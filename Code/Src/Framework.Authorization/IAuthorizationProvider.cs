using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Framework.Authorization
{
    public interface IAuthorizationProvider 
    {
        bool HasPermission(Permission permission, ClaimsIdentity user);
    }
}
