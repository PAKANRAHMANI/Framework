using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Framework.Core;

namespace Framework.AspNetCore
{
    public class HttpContextUserResolverAdapter : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserResolverAdapter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal Get()
        {
            return _httpContextAccessor.HttpContext?.User;
        }
    }
}
