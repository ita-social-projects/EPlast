using System.ComponentModel;

namespace EPlast.Resources
{
    public enum ApproveType
    {
        [Description("Поручення дійсних членів")]
        PlastMember,
        [Description("Поручення куреня УСП/УПС")]
        Club,
        [Description("Поручення Голови осередку/Осередкового УСП/УПС")]
        City
    }
}
