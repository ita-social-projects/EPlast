using EPlast.BusinessLogicLayer.Interfaces;
using System;

namespace EPlast.BusinessLogicLayer.Services
{
    public class DateTimeHelper : IDateTimeHelper
    {
        DateTime IDateTimeHelper.GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
