using System.ComponentModel;

namespace EPlast.ViewModels.AnnualReport
{
    public enum AnnualReportStatusViewModel
    {
        [Description("Непідтверджений")]
        Unconfirmed,

        [Description("Підтверджений")]
        Confirmed,

        [Description("Збережений")]
        Saved
    }
}