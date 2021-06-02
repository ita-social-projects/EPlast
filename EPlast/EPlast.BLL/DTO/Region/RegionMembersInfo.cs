using System.ComponentModel;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Region
{
    public class RegionMembersInfo
    {
        public int CityAnnualReportId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public AnnualReportStatus ReportStatus { get; set; }

        [Description("Кількість гніздечок пташат")]
        public int NumberOfSeatsPtashat { get; set; }

        [Description("Кількість самостійних роїв")] 
        public int NumberOfIndependentRiy { get; set; }

        [Description("Кількість куренів у станиці/паланці (окрузі/регіоні)")]
        public int NumberOfClubs { get; set; }

        [Description("Кількість самостійних гуртків")]
        public int NumberOfIndependentGroups { get; set; }

        [Description("Кількість діючих виховників (з усіх членів УСП, УПС)")]
        public int NumberOfTeachers { get; set; }

        [Description("Кількість адміністраторів (в проводах будь якого рівня)")]
        public int NumberOfAdministrators { get; set; }

        [Description("Кількість тих, хто поєднує виховництво та адміністрування")]
        public int NumberOfTeacherAdministrators { get; set; }

        [Description("Кількість пільговиків")]
        public int NumberOfBeneficiaries { get; set; }

        [Description("Кількість членів Пластприяту")]
        public int NumberOfPlastpryiatMembers { get; set; }

        [Description("Кількість почесних членів")]
        public int NumberOfHonoraryMembers { get; set; }

        public MembersStatistic MembersStatistic { get; set; }
    }
}
