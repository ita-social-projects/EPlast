using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;

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

        public void CheckRoles(string role, List<string> accessLevels)
        {
            var RolesDictionary = new Dictionary<string, Action>
            {
                [Roles.Admin] = () => { accessLevels.Add(AccessLevelTypeDto.Admin.GetDescription()); },
                [Roles.GoverningBodyAdmin] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.GoverningBodyAdmin.GetDescription());
                },
                [Roles.GoverningBodyHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodyHead.GetDescription());
                },
                [Roles.GoverningBodySecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodySecretary.GetDescription());
                },
                [Roles.GoverningBodySectorHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodySectorHead.GetDescription());
                },
                [Roles.GoverningBodySectorSecretary] = () =>
                {
                    accessLevels.Add(
                        AccessLevelTypeDto.LeadershipMemberForGoverningBodySectorSecretary.GetDescription());
                },
                [Roles.KurinHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinHead.GetDescription());
                },
                [Roles.KurinHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinHeadDeputy.GetDescription());
                },
                [Roles.KurinSecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinSecretary.GetDescription());
                },
                [Roles.CityHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCityHead.GetDescription());
                },
                [Roles.CityHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCityHeadDeputy.GetDescription());
                },
                [Roles.CitySecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCitySecretary.GetDescription());
                },
                [Roles.OkrugaHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaHead.GetDescription());
                },
                [Roles.OkrugaHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaHeadDeputy.GetDescription());
                },
                [Roles.OkrugaSecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaSecretary.GetDescription());
                },
                [Roles.RegisteredUser] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.RegisteredUser.GetDescription());
                },
                [Roles.Supporter] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.Supporter.GetDescription());
                },
                [Roles.FormerPlastMember] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.FormerPlastMember.GetDescription());
                },
                [Roles.PlastMember] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.PlastMember.GetDescription());
                },
                [Roles.OkrugaReferentUPS] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.OkrugaReferentUPS.GetDescription());
                },
                [Roles.OkrugaReferentUSP] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.OkrugaReferentUSP.GetDescription());
                },
                [Roles.OkrugaReferentOfActiveMembership] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.OkrugaReferentOfActiveMembership.GetDescription());
                },
                [Roles.CityReferentUPS] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.CityReferentUPS.GetDescription());
                },
                [Roles.CityReferentUSP] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.CityReferentUSP.GetDescription());
                },
                [Roles.CityReferentOfActiveMembership] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.CityReferentOfActiveMembership.GetDescription());
                },
                [Roles.EventAdministrator] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.EventAdministrator.GetDescription());
                },
                [Roles.PlastHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.PlastHead.GetDescription());
                },
                [Roles.Interested] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDto.Interested.GetDescription());
                }
            };
            RolesDictionary[role].Invoke();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId)
        {
            var accessLevels = new List<string>();
            var user = await _userManagerService.FindByIdAsync(userId);
            var userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
            var userPlastDegree = await _plastDegreeService.GetUserPlastDegreeAsync(userId);
            if (userPlastDegree != null)
            {
                var isInSupporterDegree = userPlastDegree.PlastDegree.Name == DegreeLevels.SupporterLevelDegree;
                if (isInSupporterDegree)
                {
                    accessLevels.Add(AccessLevelTypeDto.Supporter.GetDescription());
                }
            }
           
            var activeRoles = userRoles.Intersect(Roles.ListOfRoles);
            foreach (var role in activeRoles)
            {
               CheckRoles(role, accessLevels);
            }
 
            return accessLevels.AsEnumerable();
        }
    }
}
