using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

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