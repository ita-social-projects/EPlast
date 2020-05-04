using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public enum DecesionStatusType
    {
        [Description("У розгляді")]
        InReview,

        [Description("Підтверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}