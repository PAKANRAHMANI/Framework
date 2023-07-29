using System;
using System.Collections.Generic;
using System.Linq;
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

        public TKey GetUserIdFromNameIdentifier<TKey>()
        {
           var sub = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
           return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }
        public TKey GetUserIdFromSub<TKey>()
        {
            var sub = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }
    }
}
