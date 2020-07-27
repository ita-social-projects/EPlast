using EPlast.BLL.DTO.EventCalendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventCalendar
{
    public interface IEventCalendarService
    {
        Task<List<EventCalendarInfoDTO>> GetAllActions();

        Task<List<EventCalendarInfoDTO>> GetAllEducations();

        Task<List<EventCalendarInfoDTO>> GetAllCamps();
    }
}
