using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories.Interfaces.Events;

namespace EPlast.DataAccess.Repositories.Realizations.Events
{
    public class EventCategoryTypeRepository : RepositoryBase<EventCategoryType>, IEventCategoryTypeRepository
    {
        public EventCategoryTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
