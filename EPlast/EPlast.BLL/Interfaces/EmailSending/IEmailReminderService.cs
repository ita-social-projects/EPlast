using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IEmailReminderService
    {
        Task JoinCityReminder();
    }
}
