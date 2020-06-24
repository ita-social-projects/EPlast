using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EPlast.BusinessLogicLayer.DTO.AnnualReport
{
    public enum CityLegalStatusTypeDTO
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
