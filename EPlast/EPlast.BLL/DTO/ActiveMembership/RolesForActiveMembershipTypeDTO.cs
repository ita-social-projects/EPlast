using System.ComponentModel;
using EPlast.Resources;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum RolesForActiveMembershipTypeDTO
    {

        [Description("Зареєстрований користувач")]
        RegisteredUser,

        [Description("Колишній член пласту")]
        ExPlastMember,

        [Description("Прихильник")]
        Supporter,

        [Description("Пластун")]
        Plastun
    }
}
