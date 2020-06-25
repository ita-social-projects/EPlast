using EPlast.BLL.Interfaces;
using System;

namespace EPlast.BLL.Services
{
    public class DateTimeHelper : IDateTimeHelper
    {
        DateTime IDateTimeHelper.GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
