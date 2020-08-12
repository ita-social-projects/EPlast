using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.ActiveMembership
{
    public class AccessLevelService : IAccessLevelService
    {
        private readonly IUserManagerService _userManagerService;
        public AccessLevelService(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId)
        {
            List<string> accessLevels = new List<string>();
            var user = await _userManagerService.FindByIdAsync(userId);
            List<string> userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
            if (userRoles.Count == 1)
            {
                if (userRoles[0] == RolesForActiveMembershipTypeDTO.Plastun.GetDescription())
                {
                    accessLevels.Add(AccessLevelTypeDTO.Member.GetDescription());
                }
                else if (userRoles[0] == RolesForActiveMembershipTypeDTO.Supporter.GetDescription())
                {
                    accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
                }
            }
            else if (userRoles.Count > 1)
            {
                accessLevels.AddRange(new List<string>
                {
                    AccessLevelTypeDTO.Member.GetDescription(),
                    AccessLevelTypeDTO.LeadershipMember.GetDescription()
                });
            }
            else
            {
                accessLevels.Add(AccessLevelTypeDTO.FormerMember.GetDescription());
            }

            return accessLevels.AsEnumerable();
        }
    }
}
