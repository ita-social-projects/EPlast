using System.ComponentModel;

namespace EPlast.BussinessLayer.DTO
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