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
        public const string KurinSecretary = "Діловод Куреня";
        public const string OkrugaHead = "Голова Округи";
        public const string OkrugaSecretary = "Діловод Округи";
        public const string CityHead = "Голова Станиці";
        public const string CitySecretary = "Діловод Станиці";
        public const string FormerPlastMember = "Колишній член Пласту";
        public const string RegisteredUser = "Зареєстрований користувач";
        public const string Interested = "Зацікавлений";
        public const string RegionBoardHead = "Голова Краю";

        public const string DegreeAssignRoles = Admin + "," + PlastHead + "," + EventAdministrator + "," + KurinHead + "," + KurinSecretary + "," +
            OkrugaHead + "," + OkrugaSecretary + "," + CityHead + "," + CitySecretary;
        public const string HeadsAdminAndPlastun = Admin + "," + OkrugaHead + "," + CityHead + "," + KurinHead + "," + PlastMember;
        public const string HeadsAndAdmin = Admin + "," + OkrugaHead + "," + CityHead + "," + KurinHead;
        public const string AdminAndOkrugaHead = Admin + "," + OkrugaHead;
        public const string HeadsAdminPlastunAndSupporter = Admin + "," + OkrugaHead + "," + CityHead + "," + KurinHead + "," + PlastMember + "," + Supporter;
        public const string HeadsAdminPlastunSupporterAndRegisteredUser = Admin + "," + OkrugaHead + "," + CityHead + "," + KurinHead + "," + PlastMember + "," +
            Supporter + "," + RegisteredUser;
        public const string StatisticsAccessRoles = Admin + "," + PlastHead + "," + OkrugaHead + "," + CityHead;
        public const string AdminPlastMemberAndSupporter = Admin + "," + PlastMember + "," + Supporter;
        public const string AdminAndKurinHead = Admin + "," + KurinHead;
        public const string AdminCityHeadOkrugaHead = Admin + "," + CityHead + "," + OkrugaHead;
    }
}
