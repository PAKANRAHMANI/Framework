using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Framework.Core
{
    public interface ICurrentUser
    {
        ClaimsPrincipal Get();
        TKey GetUserIdFromNameIdentifier<TKey>();
        TKey GetUserIdFromSub<TKey>();
    }
}
