using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;

namespace NoteTaker.Core.Services.Implementations
{
    public class JwtTokenService : ITokenService
    {
        private readonly Security securityOptions;

        public JwtTokenService(IOptions<Security> securityOptions)
        {
            this.securityOptions = securityOptions.Value;
        }

        public string GetToken(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(securityOptions.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
