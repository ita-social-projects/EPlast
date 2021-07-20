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
        public const string CityHead = "Голова Станиці";
        public const string CityHeadDeputy = "Заступник Голови Станиці";
        public const string CitySecretary = "Діловод Станиці";
        public const string FormerPlastMember = "Колишній член Пласту";
        public const string RegisteredUser = "Зареєстрований користувач";
        public const string Interested = "Зацікавлений";
        public const string RegionBoardHead = "Голова Краю";
        public const string GoverningBodyHead = "Голова Керівного Органу";
        public const string GoverningBodySecretary = "Діловод Керівного Органу";
        public const string GoverningBodySectorHead = "Голова Напряму Керівного Органу";
        public const string GoverningBodySectorSecretary = "Діловод Напряму Керівного Органу";

        public const string AdminAndGoverningBodySectorHead = Admin + "," + GoverningBodySectorHead;
        public const string AdminAndGoverningBodyHead = Admin + "," + GoverningBodyHead;
        public const string DegreeAssignRoles = Admin + "," + PlastHead + "," + EventAdministrator + "," + KurinHead + "," + KurinHeadDeputy + "," + KurinSecretary + "," +
            OkrugaHead + "," + OkrugaHeadDeputy + "," + OkrugaSecretary + "," + CityHead + "," + CityHeadDeputy + "," + CitySecretary;
        public const string HeadsAndHeadDeputiesAndAdminAndPlastun = Admin + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," + CityHeadDeputy + "," 
            + KurinHead + "," + KurinHeadDeputy + "," + PlastMember;
        public const string HeadsAndHeadDeputiesAndAdmin = Admin + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," + CityHeadDeputy + "," + 
            KurinHead + "," + KurinHeadDeputy;
        public const string AdminAndOkrugaHeadAndOkrugaHeadDeputy = Admin + "," + OkrugaHead + "," + OkrugaHeadDeputy;
        public const string HeadsAndHeadDeputiesAndAdminPlastunAndSupporter = Admin + "," + GoverningBodyHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," 
            + CityHeadDeputy + "," + KurinHead + "," + KurinHeadDeputy + "," + PlastMember + "," + Supporter;
        public const string HeadsAndHeadDeputiesAndAdminPlastunSupporterAndRegisteredUser = Admin + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," 
            + CityHeadDeputy + "," + KurinHead + "," + KurinHeadDeputy + "," + PlastMember + "," + Supporter + "," + RegisteredUser;
        public const string StatisticsAccessRoles = Admin + "," + PlastHead + "," + OkrugaHead + "," + OkrugaHeadDeputy + "," + CityHead + "," 
            + CityHeadDeputy;
        public const string AdminPlastMemberAndSupporter = Admin + "," + PlastMember + "," + Supporter;
        public const string AdminAndKurinHeadAndKurinHeadDeputy = Admin + "," + KurinHead + "," + KurinHeadDeputy;
        public const string AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy = Admin + "," + CityHead + "," + OkrugaHead + "," + CityHeadDeputy + "," + OkrugaHeadDeputy;
        public const string AdminRegionBoardHeadOkrugaCityHeadAndDeputy = Admin + "," + RegionBoardHead + "," + OkrugaHead + ","  + OkrugaHeadDeputy + "," 
            + CityHead + "," + CityHeadDeputy;
        public const string AdminRegionBoardHeadOkrugaHeadAndDeputy = Admin + "," + RegionBoardHead + "," + OkrugaHead + "," + OkrugaHeadDeputy;
        public const string AdminAndCityHeadAndCityHeadDeputy = Admin + "," + CityHead + "," + CityHeadDeputy;
    }
}
