using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories.Interfaces.Events;


namespace EPlast.DataAccess.Repositories.Realizations.Events
{
   public class EventSectionRepository : RepositoryBase<EventSection>, IEventSectionRepository
    {
        public EventSectionRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
