using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventCalendar;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventsManager
    {
        Task<List<EventCalendarInfoDto>> GetActionsAsync();

        Task<List<EventCalendarInfoDto>> GetEducationsAsync();

        Task<List<EventCalendarInfoDto>> GetCampsAsync();
    }
}
