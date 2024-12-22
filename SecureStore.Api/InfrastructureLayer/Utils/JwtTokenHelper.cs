using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureStore.Api.InfrastructureLayer.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureStore.Api.InfrastructureLayer.Utils
{
    public class JwtTokenHelper
    {
        private JwtSettings _jwtSettings;
        public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateToken(string Email, string UserName, string Role, int UserId)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name,UserName),
                new Claim(JwtRegisteredClaimNames.Sub,UserId.ToString()),
                new Claim(ClaimTypes.Role,Role),
            };

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials
            };

            var TokenHendler = new JwtSecurityTokenHandler();
            var Token = TokenHendler.CreateToken(TokenDescriptor);

            return TokenHendler.WriteToken(Token);
        }
    }
}
