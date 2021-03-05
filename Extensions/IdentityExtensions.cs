using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Extensions
{
    public static class IdentityExtensions
    {
        public static Guid UserId(this ClaimsPrincipal user)
        {
            var userIdAsString = user.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(userIdAsString);
        }
        
        public static string GenerateJWT(IEnumerable<Claim> claims, string signingKey, string issuer, int expiresInDays)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expires = DateTime.Now.AddDays(expiresInDays);
            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                null,
                expires,
                singingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
