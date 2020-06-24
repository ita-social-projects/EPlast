using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EPlast.Bussiness.DTO.AnnualReport
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
