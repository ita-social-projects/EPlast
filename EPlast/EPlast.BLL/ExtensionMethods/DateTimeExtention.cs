using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.ExtensionMethods
{
    public static class DateTimeExtention
    {
        public static DateTime ShiftByDifference(this DateTime currDateTime, DateTime unChangedDateTime, DateTime changedDateTime)
        {
            TimeSpan timesDifference = changedDateTime.Date - unChangedDateTime.Date;
            double daysDifference = timesDifference.TotalDays;
            return currDateTime.AddDays(daysDifference);
        }
    }
}
