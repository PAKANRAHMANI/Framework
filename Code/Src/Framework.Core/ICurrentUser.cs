using System.Security.Claims;

namespace Framework.Core
{
    public interface ICurrentUser
    {
        ClaimsPrincipal Get();
        TKey GetUserIdFromNameIdentifier<TKey>();
        TKey GetUserIdFromSub<TKey>();
        string  GetUserIp();
    }
}
