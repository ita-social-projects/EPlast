using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EPlast.BusinessLogicLayer.DTO.AnnualReport
{
    public enum AnnualReportStatusDTO
    {
        [Description("Непідтверджений")]
        Unconfirmed,

        [Description("Підтверджений")]
        Confirmed,

        [Description("Збережений")]
        Saved
    }
}
