using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Blank
{
    public class BlankAccessService : IBlankAccessService
    {
        private readonly IUserService _userService;
        private readonly UserManager<DatabaseEntities.User> _userManager;

        public BlankAccessService(IUserService userService, UserManager<DatabaseEntities.User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<bool> CanAddAchievement(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user);
        }

        public async Task<bool> CanAddBiography(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userService.GetUserAsync(user.Id);
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return
                ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.Region)) ||
                ((roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy)) && await _userService.IsUserInSameCellAsync(currentUser, focusUser, CellType.City));
        }

        public async Task<bool> CanViewAddDownloadDeleteExtractUPU(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user);
        }

        public async Task<bool> CanDeleteAchievement(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CanDeleteBiography(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CanDownloadAchievement(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user) || roles.Contains(Roles.PlastMember);
        }

        public async Task<bool> CanDownloadBiography(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user);
        }

        public async Task<bool> CanViewAchievement(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user) || roles.Contains(Roles.PlastMember);
        }

        public async Task<bool> CanViewBiography(User user, string focusUserId)
        {
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user);
        }

        public async Task<bool> CanViewBlankTab(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user) || roles.Contains(Roles.PlastMember);
        }

        public async Task<bool> CanViewListOfAchievements(User user, string focusUserId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (await IsAdminAsync(user) || user.Id == focusUserId)
            {
                return true;
            }
            return await IsOkrugaCityKurinHeadOrHisDeputy(user) || roles.Contains(Roles.PlastMember);
        }

        private async Task<bool> IsAdminAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return 
                roles.Contains(Roles.Admin) ||
                roles.Contains(Roles.GoverningBodyHead) ||
                roles.Contains(Roles.GoverningBodyAdmin);
        }

        private async Task<bool> IsOkrugaCityKurinHeadOrHisDeputy(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return
                roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy) ||
                roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy) ||
                roles.Contains(Roles.KurinHead) || roles.Contains(Roles.KurinHeadDeputy);
        }
    }
}
