using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.ActiveMembership
{
    public class AccessLevelService : IAccessLevelService
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IPlastDegreeService _plastDegreeService;

        public AccessLevelService(IPlastDegreeService plastDegreeService, IUserManagerService userManagerService)
        {
            _plastDegreeService = plastDegreeService;
            _userManagerService = userManagerService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId)
        {
            var leadershipLevelRoles = new List<string>
            {
                Roles.KurinHead,
                Roles.KurinSecretary,
                Roles.CityHead,
                Roles.CitySecretary,
                Roles.OkrugaHead,
                Roles.OkrugaSecretary,
                "Голова Керівного органу",
                "Діловод Керівного органу"
            };

            var supporterLevelDegree = "Пласт прият";
            var accessLevels = new List<string>();
            var user = await _userManagerService.FindByIdAsync(userId);
            var userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
            var userPlastDegrees = await _plastDegreeService.GetUserPlastDegreesAsync(userId);
            var isInSupporterDegree = userPlastDegrees.Any(d => d.PlastDegree.Name == supporterLevelDegree);

            if (userRoles.Contains(RolesForActiveMembershipTypeDTO.RegisteredUser.GetDescription()))
            {
                accessLevels.Add(AccessLevelTypeDTO.RegisteredUser.GetDescription());
            }

            if (userRoles.Contains(RolesForActiveMembershipTypeDTO.Supporter.GetDescription())
                     || userRoles.Contains(RolesForActiveMembershipTypeDTO.FormerPlastMember.GetDescription())
                     || isInSupporterDegree)
            {
                accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
            }

            if (userRoles.Contains(RolesForActiveMembershipTypeDTO.PlastMember.GetDescription()))
            {
                accessLevels.Add(AccessLevelTypeDTO.PlastMember.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoles).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMember.GetDescription());
            }

            return accessLevels.AsEnumerable();
        }
    }
}
