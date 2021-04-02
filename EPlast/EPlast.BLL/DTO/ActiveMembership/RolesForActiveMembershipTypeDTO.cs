using System.ComponentModel;
using EPlast.Resources;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum RolesForActiveMembershipTypeDTO
    {

        [Description(Roles.RegisteredUser)]
        RegisteredUser,

        [Description(Roles.FormerPlastMember)]
        FormerPlastMember,

        [Description(Roles.Supporter)]
        Supporter,

        [Description(Roles.PlastMember)]
        PlastMember
    }
}
