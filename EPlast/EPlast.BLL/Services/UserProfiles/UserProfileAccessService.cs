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

        public UserProfileAccessService(IUserService userService, UserManager<DatabaseEntities.User> userManager)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<bool> CanApproveAsHead(User user, string focusUserId, string role)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var focusUserRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(focusUserId));
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user) && focusUserRoles.Contains(Roles.Supporter))
            {
                return true;
            }

            return role switch
            {
                Roles.CityHead =>
                    (roles.Contains(Roles.CityHead) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.City)),
                Roles.KurinHead =>
                    (roles.Contains(Roles.KurinHead) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Club)),
                _ => false,
            };
        }

        public async Task<bool> CanEditUserProfile(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user))
            {
                return true;
            }
            return
                ((roles.Contains(Roles.OkrugaHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Region)) ||
                ((roles.Contains(Roles.CityHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.City)) ||
                ((roles.Contains(Roles.KurinHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Club));
        }

        public async Task<bool> CanViewFullProfile(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            if (roles.Contains(Roles.RegisteredUser) || roles.Contains(Roles.Supporter))
            {
                return false;
            }
            return
                (_userService.IsUserSameCity(currentUser, focusUser) || _userService.IsUserSameClub(currentUser, focusUser)) ||
                ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && _userService.IsUserSameRegion(currentUser, focusUser));
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(Roles.Admin) || 
                roles.Contains(Roles.GoverningBodyHead) || 
                roles.Contains(Roles.GoverningBodyAdmin);
        }
    }
}