using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EPlast.BLL.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        public JwtService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateJWTToken(UserDTO userDTO)
        {
            var claims = new[] {
                             new Claim(ClaimTypes.Name, userDTO.Email),
                             new Claim(JwtRegisteredClaimNames.NameId, userDTO.Id),
                             new Claim(JwtRegisteredClaimNames.FamilyName, userDTO.Id),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _jwtOptions.Issuer,
              audience: _jwtOptions.Audience,
              claims: claims,
              expires: DateTime.Now.AddMinutes(_jwtOptions.Time),
              signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
