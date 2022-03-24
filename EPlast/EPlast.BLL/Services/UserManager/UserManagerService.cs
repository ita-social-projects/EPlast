using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;
      
        private readonly IMapper _mapper;
        private readonly ILoggerService<UserManagerService> _loggerService;

        public UserManagerService(
            UserManager<User> userManager, 
            IMapper mapper,
            ILoggerService<UserManagerService> loggerService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <inheritdoc />
        public async Task<bool> IsInRoleAsync(UserDTO user, params string[] roles)
        {

            var userFirst = _mapper.Map<UserDTO, User>(user);

            foreach (var i in roles)
            {
                if (await _userManager.IsInRoleAsync(userFirst, i))
                {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<UserDTO> FindByIdAsync(string userId)
        {
            var result = _mapper.Map<User, UserDTO>(await _userManager.FindByIdAsync(userId));
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetRolesAsync(UserDTO user)
        {
            var result = await _userManager.GetRolesAsync(_mapper.Map<UserDTO, User>(user));
            return result;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteRolesAsync(User user, IEnumerable<string> roles)
        { 
           var result =  await _userManager.RemoveFromRolesAsync(user, roles);
           return result;
        }

        /// <inheritdoc />
        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            try
            {
                return await _userManager.GetUserAsync(principal);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error getting current user {exception}");
                return default;
            }
        }

        /// <inheritdoc />
        public string GetCurrentUserId(ClaimsPrincipal principal)
        {
            try
            {
                return _userManager.GetUserId(principal);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error getting current userId {exception}");
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error getting the roles of user {exception}");
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<User> FindUserByIdAsync(string userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error finding user by Id {exception}");
                return default;
            }
        }
        
        /// <inheritdoc />
        public async Task<User> FindUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error finding user by email {exception}");
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsUserInRoleAsync(User user, string role)
        {
            try
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error checking the role of user {exception}");
                return default;
            }
        }
    }
}
