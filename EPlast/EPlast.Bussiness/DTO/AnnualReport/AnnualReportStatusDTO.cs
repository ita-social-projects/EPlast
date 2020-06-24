using System.ComponentModel;

namespace EPlast.BusinessLogicLayer.DTO.AnnualReport
{
    public enum AnnualReportStatusDTO
    {
        [Description("Непідтверджений")]
        Unconfirmed,

        [Description("Підтверджений")]
        Confirmed,

        [Description("Збережений")]
        Saved
    }
}
