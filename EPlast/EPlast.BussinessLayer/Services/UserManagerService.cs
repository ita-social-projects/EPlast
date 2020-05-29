using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserManagerService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> IsInRole(UserDTO user, params string[] roles)
        {

            var user_first = _mapper.Map<UserDTO, User>(user);

            foreach (var i in roles)
            {
                if (await _userManager.IsInRoleAsync(user_first, i))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> IsInRole(ClaimsPrincipal user, params string[] roles)
        {

            var user_first = await _userManager.GetUserAsync(user);

            foreach (var i in roles)
            {
                if (await _userManager.IsInRoleAsync(user_first, i))
                {
                    return false;
                }
            }
            return true;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            var id = _userManager.GetUserId(user);
            return id;
        }

        public async Task<UserDTO> FindById(string userId)
        {
            var result = _mapper.Map<User, UserDTO>(await _userManager.FindByIdAsync(userId));
            return result;
        }
        public async Task<IEnumerable<string>> GetRoles(UserDTO user)
        {
            var result = await _userManager.GetRolesAsync(_mapper.Map<UserDTO, User>(user));
            return result;
        }
    }
}
