using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserProfileAccessService : IUserProfileAccessService
    {

        private readonly IUserService _userService;
        private readonly UserManager<DatabaseEntities.User> _userManager;

        private async Task<bool> IsAdminAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(Roles.Admin) || roles.Contains(Roles.GoverningBodyHead);
        }

        public UserProfileAccessService(IUserService userService, UserManager<DatabaseEntities.User> userManager)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<bool> ApproveAsCityHead(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if ((roles.Contains(Roles.CityHead) && _userService.IsUserSameCity(currentUser, focusUser)))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ApproveAsClubHead(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if ((roles.Contains(Roles.KurinHead) || roles.Contains(Roles.KurinHeadDeputy)) && _userService.IsUserSameClub(currentUser, focusUser))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> EditUserProfile(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && _userService.IsUserSameRegion(currentUser, focusUser))
            {
                return true;
            }
            if ((roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy)) && _userService.IsUserSameCity(currentUser, focusUser))
            {
                return true;
            }
            if ((roles.Contains(Roles.KurinHead) || roles.Contains(Roles.KurinHeadDeputy)) && _userService.IsUserSameClub(currentUser, focusUser))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ViewFullProfile(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            if (_userService.IsUserSameCity(currentUser, focusUser) || _userService.IsUserSameClub(currentUser, focusUser))
            {
                return true;
            }
            if ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && _userService.IsUserSameRegion(currentUser, focusUser))
            {
                return true;
            }

            return false;
        }
    }
}
