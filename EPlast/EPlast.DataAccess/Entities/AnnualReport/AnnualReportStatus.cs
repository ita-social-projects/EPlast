using System.ComponentModel;

namespace EPlast.DataAccess.Entities
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
