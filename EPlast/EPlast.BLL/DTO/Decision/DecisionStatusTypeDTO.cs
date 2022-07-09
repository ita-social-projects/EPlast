using System.ComponentModel;

namespace EPlast.BLL.DTO
{
    public enum DecisionStatusTypeDto
    {
        [Description("У розгляді")]
        InReview,

        [Description("Потверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}