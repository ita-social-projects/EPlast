using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class EventSectionManager : IEventSectionManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventSectionManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventSectionDTO>> GetEventSectionsDTOAsync()
        {
            var eventSections = await _repoWrapper.EventSection.GetAllAsync();
            var dto = eventSections
                .Select(eventSection => new EventSectionDTO()
                {
                    EventSectionId = eventSection.ID,
                    EventSectionName = eventSection.EventSectionName
                });

            return dto;
        }
    }
}
