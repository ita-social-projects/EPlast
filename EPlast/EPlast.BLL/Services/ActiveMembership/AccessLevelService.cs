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
            var RolesDictionary = new Dictionary<string, Action>();
            RolesDictionary[Roles.Admin] = () => { accessLevels.Add(AccessLevelTypeDto.Admin.GetDescription()); };
            RolesDictionary[Roles.GoverningBodyAdmin] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.GoverningBodyAdmin.GetDescription());
            };
            RolesDictionary[Roles.GoverningBodyHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodyHead.GetDescription());
            };
            RolesDictionary[Roles.GoverningBodySecretary] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodySecretary.GetDescription());
            };
            RolesDictionary[Roles.GoverningBodySectorHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForGoverningBodySectorHead.GetDescription());
            };
            RolesDictionary[Roles.GoverningBodySectorSecretary] = () =>
            {
                accessLevels.Add(
                    AccessLevelTypeDto.LeadershipMemberForGoverningBodySectorSecretary.GetDescription());
            };
            RolesDictionary[Roles.KurinHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinHead.GetDescription());
            };
            RolesDictionary[Roles.KurinHeadDeputy] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinHeadDeputy.GetDescription());
            };
            RolesDictionary[Roles.KurinSecretary] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForKurinSecretary.GetDescription());
            };
            RolesDictionary[Roles.CityHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCityHead.GetDescription());
            };
            RolesDictionary[Roles.CityHeadDeputy] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCityHeadDeputy.GetDescription());
            };
            RolesDictionary[Roles.CitySecretary] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForCitySecretary.GetDescription());
            };
            RolesDictionary[Roles.OkrugaHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaHead.GetDescription());
            };
            RolesDictionary[Roles.OkrugaHeadDeputy] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaHeadDeputy.GetDescription());
            };
            RolesDictionary[Roles.OkrugaSecretary] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.LeadershipMemberForOkrugaSecretary.GetDescription());
            };
            RolesDictionary[Roles.RegisteredUser] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.RegisteredUser.GetDescription());
            };
            RolesDictionary[Roles.Supporter] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.Supporter.GetDescription());
            };
            RolesDictionary[Roles.FormerPlastMember] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.FormerPlastMember.GetDescription());
            };
            RolesDictionary[Roles.PlastMember] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.PlastMember.GetDescription());
            };
            RolesDictionary[Roles.OkrugaReferentUPS] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.OkrugaReferentUPS.GetDescription());
            };
            RolesDictionary[Roles.OkrugaReferentUSP] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.OkrugaReferentUSP.GetDescription());
            };
            RolesDictionary[Roles.OkrugaReferentOfActiveMembership] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.OkrugaReferentOfActiveMembership.GetDescription());
            };
            RolesDictionary[Roles.CityReferentUPS] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.CityReferentUPS.GetDescription());
            };
            RolesDictionary[Roles.CityReferentUSP] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.CityReferentUSP.GetDescription());
            };
            RolesDictionary[Roles.CityReferentOfActiveMembership] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.CityReferentOfActiveMembership.GetDescription());
            };
            RolesDictionary[Roles.EventAdministrator] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.EventAdministrator.GetDescription());
            };
            RolesDictionary[Roles.PlastHead] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.PlastHead.GetDescription());
            };
            RolesDictionary[Roles.Interested] = () =>
            {
                accessLevels.Add(AccessLevelTypeDto.Interested.GetDescription());
            };
            RolesDictionary[role].Invoke();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId)
        {
            var accessLevels = new List<string>();
            var user = await _userManagerService.FindByIdAsync(userId);
            var userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
           
            var activeRoles = userRoles.Intersect(Roles.ListOfRoles);
            foreach (var role in activeRoles)
            {
               CheckRoles(role, accessLevels);
            }
 
            return accessLevels.AsEnumerable();
        }
    }
}
