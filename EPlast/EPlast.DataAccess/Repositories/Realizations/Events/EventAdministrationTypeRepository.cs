using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories.Interfaces.Events;

namespace EPlast.DataAccess.Repositories
{
    public class EventAdministrationTypeRepository : RepositoryBase<EventAdministrationType>, IEventAdministrationTypeRepository
    {
        public EventAdministrationTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}