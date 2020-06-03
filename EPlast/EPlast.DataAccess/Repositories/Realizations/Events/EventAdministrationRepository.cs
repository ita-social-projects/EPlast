using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.DataAccess.Repositories
{
    public class EventAdministrationRepository : RepositoryBase<EventAdministration>, IEventAdministrationRepository
    {
        public EventAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}