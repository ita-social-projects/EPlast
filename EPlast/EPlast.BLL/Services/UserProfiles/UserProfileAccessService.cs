using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
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
            var currentUserRoles = await _userManager.GetRolesAsync(user);
            var focusUserRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(focusUserId));
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (IsUserAdmin(currentUserRoles) && focusUserRoles.Contains(Roles.Supporter))
            {
                return true;
            }

            return role switch
            {
                Roles.CityHead =>
                    (currentUserRoles.Contains(Roles.CityHead) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.City)) ||
                    (currentUserRoles.Contains(Roles.OkrugaHead) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Region)),
                Roles.KurinHead =>
                    (currentUserRoles.Contains(Roles.KurinHead) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Club)),
                _ => false,
            };
        }

        public async Task<bool> CanEditUserProfile(User user, string focusUserId)
        {
            if (user.Id == focusUserId)
            {
                return true;
            }

            var currentUserRoles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (IsUserAdmin(currentUserRoles))
            {
                return true;
            }
            return
                ((currentUserRoles.Contains(Roles.OkrugaHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Region)) ||
                ((currentUserRoles.Contains(Roles.CityHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.City)) ||
                ((currentUserRoles.Contains(Roles.KurinHead)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Club));
        }

        public async Task<bool> CanViewFullProfile(User user, string focusUserId)
        {
            if (user.Id == focusUserId)
            {
                return true;
            }

            var currentUserRoles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (IsUserAdmin(currentUserRoles))
            {
                return true;
            }

            if (IsRegionAdmin(currentUserRoles) && _userService.IsUserSameRegion(currentUser, focusUser))
            {
                return true;
            }

            if (IsCityAdmin(currentUserRoles) && _userService.IsUserSameCity(currentUser, focusUser))
            {
                return true;
            }

            if (currentUserRoles.Contains(Roles.RegisteredUser) || currentUserRoles.Contains(Roles.Supporter))
            {
                return false;
            }

            return
                _userService.IsUserSameCity(currentUser, focusUser) ||
                 _userService.IsUserSameClub(currentUser, focusUser);
        }

        private bool IsRegionAdmin(IList<string> userRoles)
        {
            return userRoles.Contains(Roles.OkrugaHead)
                   || userRoles.Contains(Roles.OkrugaHeadDeputy)
                   || userRoles.Contains(Roles.OkrugaReferentUPS)
                   || userRoles.Contains(Roles.OkrugaReferentUSP)
                   || userRoles.Contains(Roles.OkrugaReferentOfActiveMembership);
        }

        private bool IsCityAdmin(IList<string> userRoles)
        {
            return userRoles.Contains(Roles.CityHead)
                   || userRoles.Contains(Roles.CityHeadDeputy)
                   || userRoles.Contains(Roles.CityReferentUPS)
                   || userRoles.Contains(Roles.CityReferentUSP)
                   || userRoles.Contains(Roles.CityReferentOfActiveMembership); ;
        }
        private bool IsUserAdmin(IList<string> userRoles)
        {
            return userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin);
        }
    }
}