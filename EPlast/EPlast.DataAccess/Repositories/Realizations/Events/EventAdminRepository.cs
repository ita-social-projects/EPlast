using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

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