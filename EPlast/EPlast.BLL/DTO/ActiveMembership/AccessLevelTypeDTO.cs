using System.ComponentModel;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum AccessLevelTypeDTO
    {
        [Description("Зареєстрований користувач")]
        RegisteredUser,

        [Description("Доступ прихильника організації")]
        Supporter,

        [Description("Доступ члена організації")]
        PlastMember,

        [Description("Доступ члена проводу організації")]
        LeadershipMember,

        [Description("Доступ колишнього члена організації")]
        FormerPlastMember
    }
}
