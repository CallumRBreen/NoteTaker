using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        [Fact]
        public void If_UserId_Is_Null_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => service.GetToken(null));
        }

        [Fact]
        public void Create_Token_From_UserId_Successfully()
        {
            var userId = Guid.NewGuid().ToString();

            var tokenString = service.GetToken(userId);

            var token = GetJwtSecurityToken(tokenString);

            Assert.Equal(userId, token.Claims.First(x => x.Type.Equals("unique_name")).Value);
        }

        private JwtSecurityToken GetJwtSecurityToken(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}
