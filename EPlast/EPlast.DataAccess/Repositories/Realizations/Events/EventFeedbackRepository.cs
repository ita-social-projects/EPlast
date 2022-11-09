using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories.Interfaces.Events;

namespace EPlast.DataAccess.Repositories
{
    public class EventFeedbackRepository : RepositoryBase<EventFeedback>, IEventFeedbackRepository
    {
        public EventFeedbackRepository(EPlastDBContext _dbContext) : base(_dbContext) { }
    }
}
