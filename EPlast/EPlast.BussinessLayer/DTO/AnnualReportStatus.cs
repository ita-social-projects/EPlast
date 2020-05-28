using System.ComponentModel;

namespace EPlast.BussinessLayer.DTO
{
    public enum AnnualReportStatus
    {
        [Description("Непідтверджений")]
        Unconfirmed,

        [Description("Підтверджений")]
        Confirmed,

        [Description("Збережений")]
        Saved
    }
}