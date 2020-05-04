using EPlast.DataAccess.Entities;

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