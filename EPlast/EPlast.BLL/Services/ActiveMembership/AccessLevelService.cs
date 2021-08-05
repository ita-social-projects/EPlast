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
            var leadershipLevelRoleForGoverningBodyHead = new List<string>
            {
                Roles.GoverningBodyHead
            };

            var leadershipLevelRoleForSectorHead = new List<string>
            {
                Roles.GoverningBodySectorHead
            };

            var leadershipLevelRoleForGoverningBodySecretary = new List<string>
            {
                Roles.GoverningBodySecretary
            };

            var leadershipLevelRoleForKurinHead = new List<string>
            {
                Roles.KurinHead
            };

            var leadershipLevelRoleForKurinHeadDeputy = new List<string>
            {
                Roles.KurinHeadDeputy
            };

            var leadershipLevelRoleForKurinSecretary = new List<string>
            {
                Roles.KurinSecretary
            };

            var leadershipLevelRoleForCityHead = new List<string>
            {
                Roles.CityHead
            };

            var leadershipLevelRoleForCityHeadDeputy = new List<string>
            {
                Roles.CityHeadDeputy
            };

            var leadershipLevelRoleForCitySecretary = new List<string>
            {
                Roles.CitySecretary
            };

            var leadershipLevelRoleForOkrugaHead = new List<string>
            {
                Roles.OkrugaHead
            };

            var leadershipLevelRoleForOkrugaHeadDeputy = new List<string>
            {
                Roles.OkrugaHeadDeputy
            };

            var leadershipLevelRoleForOkrugaSecretary = new List<string>
            {
                 Roles.OkrugaSecretary
            };

            var supporterLevelDegree = "Пластприят";
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
                     || isInSupporterDegree)
            {
                accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
            }

            if (userRoles.Contains(RolesForActiveMembershipTypeDTO.FormerPlastMember.GetDescription()))
            {
                accessLevels.Add(AccessLevelTypeDTO.FormerPlastMember.GetDescription());
            }

            if (userRoles.Contains(RolesForActiveMembershipTypeDTO.PlastMember.GetDescription()))
            {
                accessLevels.Add(AccessLevelTypeDTO.PlastMember.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForGoverningBodyHead).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodyHead.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForSectorHead).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySectorHead.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForGoverningBodySecretary).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySecretary.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForKurinHead).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinHead.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForKurinHeadDeputy).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinHeadDeputy.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForKurinSecretary).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinSecretary.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForCityHead).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCityHead.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForCityHeadDeputy).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCityHeadDeputy.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForCitySecretary).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCitySecretary.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForOkrugaHead).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaHead.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForOkrugaHeadDeputy).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaHeadDeputy.GetDescription());
            }

            if (userRoles.Intersect(leadershipLevelRoleForOkrugaSecretary).Any())
            {
                accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaSecretary.GetDescription());
            }

            return accessLevels.AsEnumerable();
        }
    }
}
