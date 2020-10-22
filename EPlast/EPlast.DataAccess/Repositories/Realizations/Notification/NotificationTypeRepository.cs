using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class NotificationTypeRepository : RepositoryBase<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(EPlastDBContext dbContext) : base(dbContext)
        {

        }
    }
}
