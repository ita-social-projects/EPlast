using EPlast.BLL.DTO.EventCalendar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventsManager
    {
        Task<List<EventCalendarInfoDTO>> GetEventsAsync();
    }
}
