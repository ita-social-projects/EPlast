using System.ComponentModel;
using EPlast.Resources;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum RolesForActiveMembershipTypeDTO
    {
        [Description(Roles.supporter)]
        Supporter,
        [Description(Roles.plastMember)]
        Plastun
    }
}
