using System.Security.Claims;
using Framework.Core;

namespace Framework.AspNetCore
{
    public class HttpContextUserResolverAdapter(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public ClaimsPrincipal Get()
        {
            return httpContextAccessor.HttpContext?.User;
        }

        public TKey GetUserIdFromNameIdentifier<TKey>()
        {
           var sub = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
           return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }
        public TKey GetUserIdFromSub<TKey>()
        {
            var sub = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }

        public string GetUserIp()
        {
            return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
        }
    }
}
