using System.ComponentModel;

namespace EPlast.ViewModels.AnnualReport
{
    public enum CityLegalStatusTypeViewModel
    {
        [Description("Зареєстрована юридична особа")]
        RegisteredLegalEntity,

        [Description("Легалізована шляхом повідомлення")]
        LegalizedByMessage,

        [Description("Нелегалізована у місцевих органах влади")]
        NotLegalizedInByLocalAuthorities,

        [Description("В процесі легалізації/реєстрації")]
        InTheProcessOfLegalization,

        [Description("Зареєстрований відокремлений підрозділ")]
        RegisteredSeparatedSubdivision
    }
}