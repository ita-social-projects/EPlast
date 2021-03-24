using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.Resources
{
    public static class Roles
    {
        public const string admin = "Admin";
        public const string supporter = "Прихильник";
        public const string plastMember = "Дійсний член організації";
        public const string plastHead = "Голова Пласту";
        public const string eventAdministrator = "Адміністратор подій";
        public const string kurinHead = "Голова Куреня";
        public const string kurinSecretary = "Діловод Куреня";
        public const string okrugaHead = "Голова Округи";
        public const string okrugaSecretary = "Діловод Округи";
        public const string cityHead = "Голова Станиці";
        public const string citySecretary = "Діловод Станиці";
        public const string formerPlastMember = "Колишній член пласту";
        public const string registeredUser = "Зареєстрований користувач";
        public const string interested = "Зацікавлений";

        public const string degreeAssignRoles = admin + "," + plastHead + "," + eventAdministrator + "," + kurinHead + "," + kurinSecretary + "," +
            okrugaHead + "," + okrugaSecretary + "," + cityHead + "," + citySecretary;
        public const string headsAdminAndPlastun = admin + "," + okrugaHead + "," + cityHead + "," + kurinHead + "," + plastMember;
        public const string headsAndAdmin = admin + "," + okrugaHead + "," + cityHead + "," + kurinHead;
        public const string adminAndOkrugaHead = admin + "," + okrugaHead;
        public const string headsAdminPlastunAndSupporter = admin + "," + okrugaHead + "," + cityHead + "," + kurinHead + "," + plastMember + "," + supporter;
        public const string headsAdminPlastunSupporterAndRegisteredUser = admin + "," + okrugaHead + "," + cityHead + "," + kurinHead + "," + plastMember + "," +
            supporter + "," + registeredUser;
        public const string statisticsAccessRoles = admin + "," + plastHead + "," + okrugaHead + "," + cityHead + "," + kurinHead;
        public const string adminPlastMemberAndSupporter = admin + "," + plastMember + "," + supporter;
    }
}
