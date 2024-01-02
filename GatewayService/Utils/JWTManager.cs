using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GatewayService.Utils
{
    public static class JWTManager
    {
        private static readonly string _secretKey = "4b46723a0157db12a8e00dc8cc839a63360ca1e14f83f0d40450aa1a3f876de5";

        public static string GenerateJwt(IEnumerable<Claim> claims, DateTime expires)
        {
            byte[] keyBytes1 = new byte[32]; // 256 bits
            byte[] keyBytes = Encoding.UTF8.GetBytes(_secretKey);
            string base64Key = Convert.ToBase64String(keyBytes);
            var key = new SymmetricSecurityKey(Convert.FromBase64String(base64Key));
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