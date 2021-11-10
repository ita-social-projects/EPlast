using System.ComponentModel;

namespace EPlast.BLL.DTO
{
    public enum DecisionStatusTypeDTO
    {
        [Description("У розгляді")]
        InReview,

        [Description("Потверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}