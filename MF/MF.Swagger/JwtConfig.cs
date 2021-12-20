using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MF.Swagger
{
    public class JwtConfig
    {
        public string JwtSecurityKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string JwtExpireTime { get; set; }

#pragma warning disable CA1822 // Mark members as static

        public (string, DateTime) GenToken(List<Claim> claims, string jwtSecurityKey, string jwtIssuer, string jwtAudience, string jwtExpireTime)
#pragma warning restore CA1822 // Mark members as static
        {
            var now = DateTime.Now;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            if (!double.TryParse(jwtExpireTime, out double expireTime))
            {
                expireTime = 1800;
            }
            var expires = now.AddSeconds(expireTime);
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expires);
        }

        public (string, DateTime) GenToken(List<Claim> claims)
        {
            var now = DateTime.Now;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            if (!double.TryParse(JwtExpireTime, out double expireTime))
            {
                expireTime = 1800;
            }
            var expires = now.AddSeconds(expireTime);
            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: JwtAudience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expires);
        }

#pragma warning disable CA1822 // Mark members as static

        public JwtSecurityToken GetJwtSecurityToken(string token)
#pragma warning restore CA1822 // Mark members as static
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken;
        }

#pragma warning disable CA1822 // Mark members as static

        public bool ValidatorToken(string token)
#pragma warning restore CA1822 // Mark members as static
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = ReadToken(token);
            if (!jwtToken.Audiences.Any(c => c == JwtAudience) || jwtToken.Issuer != JwtIssuer)
            {
                return false;
            }
            return true;
        }

#pragma warning disable CA1822 // Mark members as static

        public JwtSecurityToken ReadToken(string token)
#pragma warning restore CA1822 // Mark members as static
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken;
        }
    }
}