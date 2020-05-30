using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class EventGallaryRepository : RepositoryBase<EventGallary>, IEventGallaryRepository
    {
        public EventGallaryRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}