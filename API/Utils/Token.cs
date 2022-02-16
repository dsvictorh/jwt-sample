using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Utils
{
    public static class Token
    {
        public static string IssueToken(List<Claim> claims, IConfiguration configuration)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Get<string>()));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration.GetSection("JWT:Issuer").Get<string>(),
                audience: configuration.GetSection("JWT:Audience").Get<string>(),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static List<Claim> GetValidTokenClaims(string jwtToken, IConfiguration configuration)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Get<string>()));

            TokenValidationParameters validation = new TokenValidationParameters
            {
                RequireExpirationTime = false,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("JWT:Issuer").Get<string>(),
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("JWT:Audience").Get<string>(),
                RequireSignedTokens = true,
                IssuerSigningKey = key,
            };

            ClaimsPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validation, out _);
            return claims.Claims.ToList();
        }
    }
}
