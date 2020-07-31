using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public enum AccessLevelTypeDTO
    {
        [Description("Колишній член (пласт сприят)")]
        FormerMember,
        [Description("Доступ прихильника організації")]
        Supporter,
        [Description("Доступ члена організації")]
        Member,
        [Description("Доступ члена проводу організації")]
        LeadershipMember


    }
}
