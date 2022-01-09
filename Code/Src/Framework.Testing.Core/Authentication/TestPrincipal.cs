using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Framework.Testing.Core.Authentication
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(ClaimsIdentity claimsIdentity) : base(claimsIdentity)
        { }

        public override IIdentity Identity => new TestIdentity();
    }
}
