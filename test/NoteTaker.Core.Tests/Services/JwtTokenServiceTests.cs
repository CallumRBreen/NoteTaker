using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.Extensions.Options;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Implementations;
using Xunit;

namespace NoteTaker.Core.Tests.Services
{
    public class JwtTokenServiceTests
    {
        private readonly JwtTokenService service;

        public JwtTokenServiceTests()
        {
            var securityOptions = Options.Create(new Security {JwtSecret = Guid.NewGuid().ToString()});

            service = new JwtTokenService(securityOptions);
        }

        [Theory]
        [InlineData(null,"Apples")]
        [InlineData("Apples", null)]
        public void If_UserId_Is_Null_Throw_ArgumentNullException(string userId, string username)
        {
            Assert.Throws<ArgumentNullException>(() => service.GetToken(userId, username));
        }

        [Fact]
        public void Create_Token_From_UserId_Successfully()
        {
            var userId = Guid.NewGuid().ToString();
            var username = "Apples";

            var tokenString = service.GetToken(userId, username);

            var token = GetJwtSecurityToken(tokenString);

            Assert.Equal(userId, token.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.NameId)).Value);
            Assert.Equal(username, token.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.UniqueName)).Value);
        }

        private JwtSecurityToken GetJwtSecurityToken(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}
