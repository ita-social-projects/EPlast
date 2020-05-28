using EPlast.BussinessLayer.Interfaces;
using System;

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
