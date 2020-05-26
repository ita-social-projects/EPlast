using EPlast.BussinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.Services
{
    public class DateTimeHelper : IDateTimeHelper
    {
        DateTime IDateTimeHelper.GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
