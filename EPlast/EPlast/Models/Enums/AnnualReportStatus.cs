using System.ComponentModel;

namespace EPlast.Models.Enums
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