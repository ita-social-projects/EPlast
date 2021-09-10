using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using System;
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

        public void CheckRoles(string role, List<string> accessLevels)
        {
                var RolesDictionary = new Dictionary<string, Action>();
                RolesDictionary[Roles.Admin] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.Admin.GetDescription());
                };
                RolesDictionary[Roles.GoverningBodyHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodyHead.GetDescription());
                };
                RolesDictionary[Roles.GoverningBodySectorHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySectorHead.GetDescription());
                };
                RolesDictionary[Roles.GoverningBodySecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForGoverningBodySecretary.GetDescription());
                };
                RolesDictionary[Roles.KurinHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinHead.GetDescription());
                };
                RolesDictionary[Roles.KurinHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinHeadDeputy.GetDescription());
                };
                RolesDictionary[Roles.KurinSecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForKurinSecretary.GetDescription());
                };
                RolesDictionary[Roles.CityHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCityHead.GetDescription());
                };
                RolesDictionary[Roles.CityHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCityHeadDeputy.GetDescription());
                };
                RolesDictionary[Roles.CitySecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForCitySecretary.GetDescription());
                };
                RolesDictionary[Roles.OkrugaHead] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaHead.GetDescription());
                };
                RolesDictionary[Roles.OkrugaHeadDeputy] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaHeadDeputy.GetDescription());
                };
                RolesDictionary[Roles.OkrugaSecretary] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.LeadershipMemberForOkrugaSecretary.GetDescription());
                };
                RolesDictionary[Roles.RegisteredUser] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.RegisteredUser.GetDescription());
                };
                RolesDictionary[Roles.Supporter] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
                };
                RolesDictionary[Roles.FormerPlastMember] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.FormerPlastMember.GetDescription());
                };
                RolesDictionary[Roles.PlastMember] = () =>
                {
                    accessLevels.Add(AccessLevelTypeDTO.PlastMember.GetDescription());
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
            if(userPlastDegree != null)
            {
                var isInSupporterDegree = userPlastDegree.PlastDegree.Name == DegreeLevels.SupporterLevelDegree;
                if (isInSupporterDegree)
                {
                    accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
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
