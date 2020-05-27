using System.ComponentModel;

namespace EPlast.BussinessLayer.DTO
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