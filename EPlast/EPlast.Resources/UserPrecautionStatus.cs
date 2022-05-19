using System.ComponentModel;

namespace EPlast.Resources
{
    public enum UserPrecautionStatus
    {
        [Description("Прийнято")]
        Accepted,
        [Description("Потверджено")]
        Confirmed,
        [Description("Скасовано")]
        Canceled
    }
}
