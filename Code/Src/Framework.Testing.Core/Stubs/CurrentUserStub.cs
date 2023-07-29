using System;
using System.Collections.Generic;
using System.Linq;
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

        public TKey GetUserIdFromNameIdentifier<TKey>()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Email,"pakan.rahmani@gmail.com"),
                new Claim(ClaimTypes.Name,"pakan rahmani"),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var principal = new TestPrincipal(claimsIdentity);

            var sub = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }

        public TKey GetUserIdFromSub<TKey>()
        {
            var claims = new List<Claim>()
            {
                new Claim("sub","1"),
                new Claim(ClaimTypes.Email,"pakan.rahmani@gmail.com"),
                new Claim(ClaimTypes.Name,"pakan rahmani"),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var principal = new TestPrincipal(claimsIdentity);

            var sub = principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            return (TKey)Convert.ChangeType(sub, typeof(TKey));
        }
    }
}
