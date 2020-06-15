using System.ComponentModel;

namespace EPlast.WebApi.Models.Decision
{
    public enum DecisionStatusType
    {
        [Description("У розгляді")]
        InReview,

        [Description("Підтверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}
