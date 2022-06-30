using System.ComponentModel;
namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum AccessLevelTypeDTO
    {
        [Description("Адміністратор")] Admin,

        [Description("Зареєстрований користувач")]
        RegisteredUser,

        [Description("Доступ прихильника організації")]
        Supporter,

        [Description("Доступ члена організації")]
        PlastMember,

        [Description("Доступ члена проводу організації")]
        LeadershipMember,

        [Description("Крайовий Адмін")] GoverningBodyAdmin,

        [Description("Доступ члена проводу організації, Голова Керівного Органу")]
        LeadershipMemberForGoverningBodyHead,

        [Description("Доступ члена проводу організації, Голова Напряму Керівного Органу")]
        LeadershipMemberForGoverningBodySectorHead,

        [Description("Доступ члена проводу організації, Діловод Напряму Керівного Органу")]
        LeadershipMemberForGoverningBodySectorSecretary,

        [Description("Доступ члена проводу організації, Діловод Керівного Органу")]
        LeadershipMemberForGoverningBodySecretary,

        [Description("Доступ члена проводу організації, Голова Куреня")]
        LeadershipMemberForKurinHead,

        [Description("Доступ члена проводу організації, Заступник Голови Куреня")]
        LeadershipMemberForKurinHeadDeputy,

        [Description("Діловод Куреня")] LeadershipMemberForKurinSecretary,

        [Description("Доступ члена проводу організації, Голова Станиці")]
        LeadershipMemberForCityHead,

        [Description("Доступ члена проводу організації, Заступник Голови Станиці")]
        LeadershipMemberForCityHeadDeputy,

        [Description("Діловод Станиці")] LeadershipMemberForCitySecretary,

        [Description("Доступ члена проводу організації, Голова Округи")]
        LeadershipMemberForOkrugaHead,

        [Description("Доступ члена проводу організації, Заступник Голови Округи")]
        LeadershipMemberForOkrugaHeadDeputy,

        [Description("Діловод Округи")] LeadershipMemberForOkrugaSecretary,

        [Description("Доступ колишнього члена організації")]
        FormerPlastMember,

        [Description("Доступ члена проводу організації, Референт УПС Округи")]
        OkrugaReferentUPS,

        [Description("Доступ члена проводу організації, Референт УСП Округи")]
        OkrugaReferentUSP,

        [Description("Доступ члена проводу організації, Референт Дійсного Членства Станиці")]
        OkrugaReferentOfActiveMembership,

        [Description("Доступ члена проводу організації, Референт УПС Станиці")]
        CityReferentUPS,

        [Description("Доступ члена проводу організації, Референт УСП Станиці")]
        CityReferentUSP,

        [Description("Доступ члена проводу організації, Референт Дійсного Членства Станиці")]
        CityReferentOfActiveMembership
    }
}