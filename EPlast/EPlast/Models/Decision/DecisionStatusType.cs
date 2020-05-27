using System.ComponentModel;

namespace EPlast.Models
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