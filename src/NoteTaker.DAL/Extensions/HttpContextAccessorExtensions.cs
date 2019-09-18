using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NoteTaker.DAL.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static Guid GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));

            if (claim == null)
            {
                throw new InvalidOperationException($"Current user does not have a {ClaimTypes.NameIdentifier}");
            }

            return new Guid(claim.Value);
        }
    }
}
