using System.Collections.Generic;
using System.Security.Claims;
using Framework.Core;
using Framework.Testing.Core.Authentication;

namespace Framework.Testing.Core.Stubs
{
    public class CurrentUserStub : ICurrentUser
    {
        public ClaimsPrincipal Get()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Email,"pakan.rahmani@gmail.com"),
                new Claim(ClaimTypes.Name,"pakan rahmani"),
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            return new TestPrincipal(claimsIdentity);
        }
    }
}
