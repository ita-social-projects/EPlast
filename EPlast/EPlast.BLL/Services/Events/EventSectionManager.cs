using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;

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
        public async Task<int> GetSectionIdAsync(string sectionName)
        {
            var section = await _repoWrapper.EventSection
                .GetFirstAsync(predicate: es => es.EventSectionName == sectionName);

            return section.ID;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventSectionDTO>> GetEventSectionsDTOAsync()
        {
            var eventSections = await _repoWrapper.EventSection.GetAllAsync();
            var dto = eventSections
                .Select(eventSection => new EventSectionDTO()
                {
                    ID = eventSection.ID,
                    EventSectionName = eventSection.EventSectionName
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<EventSection> GetSectionByIdAsync(int id)
        {
            var eventSection = await _repoWrapper.EventSection
                .GetFirstAsync(
                    es => es.ID == id,
                    source => source
                        .Include(es => es.EventCategories)
                );

            return eventSection;
        }
    }
}