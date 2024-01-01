using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace GatewayService.Utils
{
    public class JWTManager
    {
        private static readonly string _secretKey = "theSecret";

        public static string GenerateJwt(IEnumerable<Claim> claims, DateTime expires)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        public static ClaimsPrincipal DecodeJwt(string jwtToken)
        {
            var secretKey = _secretKey;
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}