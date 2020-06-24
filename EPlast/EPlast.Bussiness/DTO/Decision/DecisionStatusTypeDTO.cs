using System.ComponentModel;

namespace EPlast.BusinessLogicLayer.DTO
{
    public enum DecisionStatusTypeDTO
    {
        [Description("У розгляді")]
        InReview,

        [Description("Підтверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}