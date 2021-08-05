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

        [Description("Доступ члена проводу організації, Голова Керівного Органу")]
        LeadershipMemberForGoverningBodyHead,

        [Description("Доступ члена проводу організації, Голова Напряму Керівного Органу")]
        LeadershipMemberForGoverningBodySectorHead,

        [Description("Доступ члена проводу організації, Діловод Керівного Органу")]
        LeadershipMemberForGoverningBodySecretary,

        [Description("Доступ члена проводу організації, Голова Куреня")]
        LeadershipMemberForKurinHead,

        [Description("Доступ члена проводу організації, Заступник Голови Куреня")]
        LeadershipMemberForKurinHeadDeputy,

        [Description("Доступ члена проводу організації, Діловод Куреня")]
        LeadershipMemberForKurinSecretary,

        [Description("Доступ члена проводу організації, Голова Станиці")]
        LeadershipMemberForCityHead,

        [Description("Доступ члена проводу організації, Заступник Голови Станиці")]
        LeadershipMemberForCityHeadDeputy,

        [Description("Доступ члена проводу організації, Діловод Станиці")]
        LeadershipMemberForCitySecretary,

        [Description("Доступ члена проводу організації, Голова Округи")]
        LeadershipMemberForOkrugaHead,

        [Description("Доступ члена проводу організації, Заступник Голови Округи")]
        LeadershipMemberForOkrugaHeadDeputy,

        [Description("Доступ члена проводу організації, Діловод Округи")]
        LeadershipMemberForOkrugaSecretary,

        [Description("Доступ колишнього члена організації")]
        FormerPlastMember
    }
}
