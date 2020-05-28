using EPlast.DataAccess.Entities;

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