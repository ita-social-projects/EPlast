using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class UserNotificationRepository : RepositoryBase<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(EPlastDBContext dbContext)
             : base(dbContext)
        {
        }
    }
}
