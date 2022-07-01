using System.Collections.Generic;

namespace EPlast.Resources
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Supporter = "Прихильник";
        public const string PlastMember = "Дійсний член організації";
        public const string PlastHead = "Голова Пласту";
        public const string EventAdministrator = "Адміністратор подій";
        public const string KurinHead = "Голова Куреня";
        public const string KurinHeadDeputy = "Заступник Голови Куреня";
        public const string KurinSecretary = "Діловод Куреня";
        public const string OkrugaHead = "Голова Округи";
        public const string OkrugaHeadDeputy = "Заступник Голови Округи";
        public const string OkrugaSecretary = "Діловод Округи";
        public const string OkrugaReferentUPS = "Референт/-ка УПС Округи";
        public const string OkrugaReferentUSP = "Референт/-ка УСП Округи";
        public const string OkrugaReferentOfActiveMembership = "Референт дійсного членства Округи";
        public const string CityHead = "Голова Станиці";
        public const string CityHeadDeputy = "Заступник Голови Станиці";
        public const string CitySecretary = "Діловод Станиці";
        public const string CityReferentUPS = "Референт/-ка УПС Станиці";
        public const string CityReferentUSP = "Референт/-ка УСП Станиці";
        public const string CityReferentOfActiveMembership = "Референт дійсного членства Станиці";
        public const string FormerPlastMember = "Колишній член Пласту";
        public const string RegisteredUser = "Зареєстрований користувач";
        public const string Interested = "Зацікавлений";
        public const string RegionBoardHead = "Голова Краю";
        public const string GoverningBodyAdmin = "Крайовий Адмін";
        public const string GoverningBodyHead = "Голова Керівного Органу";
        public const string GoverningBodySecretary = "Діловод Керівного Органу";
        public const string GoverningBodySectorHead = "Голова Напряму Керівного Органу";
        public const string GoverningBodySectorSecretary = "Діловод Напряму Керівного Органу";

        public const string AdminAndGBAdmin = Admin + "," + GoverningBodyAdmin;
        public const string AdminAndOkrugaHead = Admin + "," + GoverningBodyAdmin + "," + OkrugaHead;
        public const string AdminAndAdminsOfOkrugaAndKrayuAndCityAndKurin = Admin + "," + GoverningBodyAdmin + "," + OkrugaHead + "," + RegionBoardHead + "," + CityHead + "," + KurinHead;
        public const string AdminAndRegionBoardHead = Admin + "," + GoverningBodyAdmin + "," + RegionBoardHead;
        public const string AdminAndGBHeadAndGBSectorHead = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + GoverningBodySectorHead;

        public const string AdminAndGBHead = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead;
        public const string DegreeAssignRoles = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + PlastHead + "," + EventAdministrator + "," + KurinHead + "," + KurinHeadDeputy + "," + KurinSecretary + "," 
            + OkrugaHead + "," + OkrugaHeadDeputy + "," + OkrugaSecretary + "," + CityHead + "," + CityHeadDeputy + "," + CitySecretary;
        public const string HeadsAndHeadDeputiesAndAdminAndPlastun = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," + CityHeadDeputy + ","
            + KurinHead + "," + KurinHeadDeputy + "," + PlastMember;
        public const string HeadsAndHeadDeputiesAndAdmin = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," + CityHeadDeputy + ","
            + KurinHead + "," + KurinHeadDeputy;
        public const string AdminAndOkrugaHeadAndOkrugaHeadDeputy = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy;
        public const string HeadsAndHeadDeputiesAndAdminPlastunAndSupporter = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," 
            + CityHeadDeputy + "," + KurinHead + "," + KurinHeadDeputy + "," + PlastMember + "," + Supporter;
        public const string HeadsAndHeadDeputiesAndAdminPlastunSupporterAndRegisteredUser = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + ","
            + CityHeadDeputy + "," + KurinHead + "," + KurinHeadDeputy + "," + PlastMember + "," + Supporter + "," + RegisteredUser;
        public const string StatisticsAccessRoles = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + PlastHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + ","
            + CityHeadDeputy;
        public const string AdminPlastMemberAndSupporter = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + PlastMember + "," + Supporter;
        public const string AdminAndKurinHeadAndKurinHeadDeputy = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + KurinHead + "," + KurinHeadDeputy;

        public const string AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy = Admin +
                                                                                    "," + GoverningBodyAdmin +
                                                                                    "," + GoverningBodyHead +
                                                                                    "," + CityHead +
                                                                                    "," + OkrugaHead +
                                                                                    "," + CityHeadDeputy +
                                                                                    "," + OkrugaHeadDeputy;

        public const string AdminRegionBoardHeadOkrugaCityHeadAndDeputy = Admin +
                                                                          "," + GoverningBodyAdmin +
                                                                          "," + GoverningBodyHead +
                                                                          "," + RegionBoardHead +
                                                                          "," + OkrugaHead +
                                                                          "," + OkrugaHeadDeputy +
                                                                          "," + CityHead +
                                                                          "," + CityHeadDeputy;
        public const string AdminRegionBoardHeadOkrugaHeadAndDeputy = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + RegionBoardHead + "," + OkrugaHead + "," + OkrugaHeadDeputy;
        public const string AdminAndCityHeadAndCityHeadDeputy = Admin + "," + GoverningBodyAdmin + "," + GoverningBodyHead + "," + CityHead + "," + CityHeadDeputy;

        public const string HeadsAndHeadDeputiesAndAdminAndPlastunAndGBHeadAndGBSectorHead = Admin +
                                                                                            "," + GoverningBodyAdmin +
                                                                                            "," + GoverningBodyHead +
                                                                                            "," + OkrugaHead +
                                                                                            "," + OkrugaHeadDeputy +
                                                                                            "," + CityHead +
                                                                                            "," + CityHeadDeputy +
                                                                                            "," + KurinHead +
                                                                                            "," + KurinHeadDeputy +
                                                                                            "," + PlastMember +
                                                                                            "," + GoverningBodySectorHead;

        public const string AdminRegionBoardHeadOkrugaCityHeadAndDeputyAndReferentKurinHeadDeputyAndPlastMember = Admin +
                                                                                                                    "," + GoverningBodyAdmin +
                                                                                                                    "," + GoverningBodyHead +
                                                                                                                    "," + OkrugaHead +
                                                                                                                    "," + OkrugaHeadDeputy +
                                                                                                                    "," + CityHead +
                                                                                                                    "," + CityHeadDeputy +
                                                                                                                    "," + KurinHead +
                                                                                                                    "," + KurinHeadDeputy +
                                                                                                                    "," + PlastMember +
                                                                                                                    "," + GoverningBodySectorHead +
                                                                                                                    "," + OkrugaReferentUPS +
                                                                                                                    "," + OkrugaReferentUSP +
                                                                                                                    "," + OkrugaReferentOfActiveMembership +
                                                                                                                    "," + CityReferentUPS +
                                                                                                                    "," + CityReferentUSP +
                                                                                                                    "," + CityReferentOfActiveMembership;

        public const string CanEditCity = Admin +
                                          "," + GoverningBodyAdmin +
                                          "," + RegionBoardHead +
                                          "," + OkrugaHead +
                                          "," + OkrugaHeadDeputy +
                                          "," + OkrugaReferentUPS +
                                          "," + OkrugaReferentUSP +
                                          "," + OkrugaReferentOfActiveMembership +
                                          "," + CityHead +
                                          "," + CityHeadDeputy +
                                          "," + GoverningBodyHead +
                                          "," + CityReferentUPS +
                                          "," + CityReferentUSP +
                                          "," + CityReferentOfActiveMembership;
        public const string CanEditClub = Admin + "," + GoverningBodyAdmin + "," + RegionBoardHead + "," + KurinHead + "," + KurinHeadDeputy + "," + GoverningBodyHead;
        public const string CanCreateClub = Admin + "," + GoverningBodyAdmin + "," + RegionBoardHead + "," + GoverningBodyHead;
        public const string AdminRegionBoardHeadOkrugaCityHeadAndDeputyReferent= Admin +
                                                                                "," + GoverningBodyAdmin +
                                                                                "," + GoverningBodyHead +
                                                                                "," + OkrugaHead +
                                                                                "," + OkrugaHeadDeputy +
                                                                                "," + CityHead +
                                                                                "," + CityHeadDeputy+
                                                                                "," + CityReferentUPS +
                                                                                "," + CityReferentUSP +
                                                                                "," + CityReferentOfActiveMembership;
        public static List<string> ListOfRoles = new List<string>
        {
            Roles.Admin,
            Roles.Supporter,
            Roles.PlastMember,
            Roles.PlastHead,
            Roles.EventAdministrator,
            Roles.KurinHead,
            Roles.KurinHeadDeputy,
            Roles.KurinSecretary,
            Roles.OkrugaHead,
            Roles.OkrugaHeadDeputy,
            Roles.OkrugaSecretary,
            Roles.OkrugaReferentUPS,
            Roles.OkrugaReferentUSP,
            Roles.OkrugaReferentOfActiveMembership,
            Roles.CityHead,
            Roles.CityHeadDeputy,
            Roles.CitySecretary,
            Roles.CityReferentUPS,
            Roles.CityReferentUSP,
            Roles.CityReferentOfActiveMembership,
            Roles.FormerPlastMember,
            Roles.RegisteredUser,
            Roles.Interested,
            Roles.GoverningBodyAdmin,
            Roles.GoverningBodyHead,
            Roles.GoverningBodySecretary,
            Roles.GoverningBodySectorHead,
            Roles.GoverningBodySectorSecretary
        };

        public static List<string> DeleteableListOfRoles = new List<string>
        {
            Roles.OkrugaSecretary,
            Roles.OkrugaHeadDeputy,
            Roles.OkrugaHead,
            Roles.CitySecretary,
            Roles.CityHead,
            Roles.CityHeadDeputy
        };

        public static List<string> LowerRoles = new List<string>
        {
            Roles.RegisteredUser,
            Roles.Supporter,
            Roles.FormerPlastMember,
            Roles.Interested
        };
    }
}
