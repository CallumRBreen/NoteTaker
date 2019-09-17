using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NoteTaker.IntegrationTests.TestHelpers
{
    public static class TestJwtTokenHelper
    {
        public const string JwtSecret = "91321414-7748-4609-8032-C9E391F86776";

        public static HttpClient AddTestJwtHeader(this HttpClient httpClient)
        {
            var token = GetToken(Guid.NewGuid().ToString(), "TestUser");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return httpClient;
        }

        private static string GetToken(string userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim(JwtRegisteredClaimNames.UniqueName, username)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
