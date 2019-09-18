using System.Security.Claims;

namespace NoteTaker.Core.Tests.Helpers
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }
}
