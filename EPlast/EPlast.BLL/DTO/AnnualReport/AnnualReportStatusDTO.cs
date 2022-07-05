using System.ComponentModel;

namespace EPlast.BLL.DTO.AnnualReport
{
    public enum AnnualReportStatusDto
    {
        [Description("Непідтверджений")]
        Unconfirmed,

        [Description("Підтверджений")]
        Confirmed,

        [Description("Збережений")]
        Saved
    }
}
