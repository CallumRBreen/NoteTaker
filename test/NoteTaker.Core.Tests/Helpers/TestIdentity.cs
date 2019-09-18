using System.Security.Claims;

namespace NoteTaker.Core.Tests.Helpers
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }
}
