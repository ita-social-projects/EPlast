using System.ComponentModel;

namespace EPlast.BLL.DTO.Statistics
{
    public enum StatisticsItemIndicator
    {
        [Description("Кількість пташат")]
        NumberOfPtashata,

        [Description("Кількість новацтва")]
        NumberOfNovatstva,

        [Description("Кількість юнацтва загалом")]
        NumberOfUnatstva,

        [Description("Кількість неіменованих")]
        NumberOfUnatstvaNoname,

        [Description("Кількість прихильників")]
        NumberOfUnatstvaSupporters,

        [Description("Кількість учасників")]
        NumberOfUnatstvaMembers,

        [Description("Кількість розвідувачів")]
        NumberOfUnatstvaProspectors,

        [Description("Кількість скобів/вірлиць")]
        NumberOfUnatstvaSkobVirlyts,

        [Description("Кількість старших пластунів загалом")]
        NumberOfSenior,

        [Description("Кількість старших пластунів прихильників")]
        NumberOfSeniorPlastynSupporters,

        [Description("Кількість старших пластунів учасників")]
        NumberOfSeniorPlastynMembers,

        [Description("Кількість сеньйорів загалом")]
        NumberOfSeigneur,

        [Description("Кількість сеньйорів пластунів прихильників")]
        NumberOfSeigneurSupporters,

        [Description("Кількість сеньйорів пластунів учасників")]
        NumberOfSeigneurMembers
    }
}