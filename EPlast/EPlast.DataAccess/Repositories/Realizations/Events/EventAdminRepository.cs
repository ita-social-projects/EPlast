using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories.Interfaces.Events;

namespace EPlast.DataAccess.Repositories
{
    public class EventAdminRepository : RepositoryBase<EventAdmin>, IEventAdminRepository
    {
        public EventAdminRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}