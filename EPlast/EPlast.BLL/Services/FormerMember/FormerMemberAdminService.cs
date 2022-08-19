using System.Threading.Tasks;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.FormerMember;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.FormerMember
{
    public class FormerMemberAdminService : IFormerMemberAdminService
    {
        private readonly ICityParticipantsService _cityParticipants;
        private readonly IClubParticipantsService _clubParticipants;
        private readonly IGoverningBodyAdministrationService _governingBodyAdministrationService;
        private readonly IRegionAdministrationService _regionAdministrationService;
        private readonly ISectorAdministrationService _sectorAdministrationService;
        private readonly UserManager<User> _userManager;

        public FormerMemberAdminService(IClubParticipantsService clubParticipants,
                                        ICityParticipantsService cityParticipants,
                                        IGoverningBodyAdministrationService governingBodyAdministrationService,
                                        IRegionAdministrationService regionAdministrationService,
                                        ISectorAdministrationService sectorAdministrationService,
                                        UserManager<User> userManager)
        {
            _cityParticipants = cityParticipants;
            _clubParticipants = clubParticipants;
            _governingBodyAdministrationService = governingBodyAdministrationService;
            _regionAdministrationService = regionAdministrationService;
            _sectorAdministrationService = sectorAdministrationService;
            _userManager = userManager;
        }

        public async Task RemoveFromAdminRolesAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            }

            await _clubParticipants.RemoveAdminRolesByUserIdAsync(userId);
            await _cityParticipants.RemoveAdminRolesByUserIdAsync(userId);
            await _regionAdministrationService.RemoveAdminRolesByUserIdAsync(userId);
            await _governingBodyAdministrationService.RemoveGbAdminRoleAsync(userId);
            await _sectorAdministrationService.RemoveAdminRolesByUserIdAsync(userId);
        }
    }
}
