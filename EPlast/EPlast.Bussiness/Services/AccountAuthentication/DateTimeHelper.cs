using EPlast.Bussiness.Interfaces;
using System;

namespace EPlast.Bussiness.Services
{
    public class DateTimeHelper : IDateTimeHelper
    {
        DateTime IDateTimeHelper.GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
