using AsistenteCompras_API.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AsistenteCompras_API.Domain.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Surname, usuario.Apellido),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetConnectionString("Key")));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
