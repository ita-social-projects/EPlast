using System.ComponentModel;

namespace EPlast.DataAccess.Entities
{
    public enum UserPlastDegreeType
    {
        [Description("старший пластун прихильник")]
        SeniorPlastynSupporter,

        [Description("старший пластун")]
        SeniorPlastynMember,

        [Description("сеньйор пластун прихильник")]
        SeigneurSupporter,

        [Description("сеньйор пластун")]
        SeigneurMember
    }
}