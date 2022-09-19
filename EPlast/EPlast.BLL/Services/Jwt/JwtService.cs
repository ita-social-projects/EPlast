using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EPlast.BLL.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagerService _userManagerService;
        private readonly UserManager<User> _userManager;

        public JwtService(IConfiguration configuration, IUserManagerService userManagerService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManagerService = userManagerService;
            _userManager = userManager;
        }

        ///<inheritdoc/>
        public async Task<string> GenerateJWTTokenAsync(UserDto userDTO)
        {
            var id = userDTO.Id;
            var roles = await _userManagerService.GetRolesAsync(userDTO);

            return Generate(id, roles);
        }

        public async Task<string> GenerateJWTTokenAsync(User user)
        {
            var id = user.Id;
            var roles = await _userManager.GetRolesAsync(user);

            return Generate(id, roles);
        }

        private string Generate(string id, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                // Note 1: Use ClaimTypes.NameIdentifier instead of JwtRegisteredClaimNames.NameId in any future projects
                //
                // Note 2: Currently it is not possible to change it to ClaimTypes.NameIdentifier, because 
                // JwtRegisteredClaimNames.NameId is used everywhere on frontend, in every component, to fetch
                // information by stored user's ID in JWT. 
                // ...While using Cookie authorization at the same time. 
                // ...Don't ask me why it is that way.
                new Claim(JwtRegisteredClaimNames.NameId, id)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtIssuerSigningKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(120),
              signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
