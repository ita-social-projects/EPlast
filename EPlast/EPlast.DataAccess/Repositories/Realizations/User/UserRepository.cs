using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(EPlastDBContext dbContext) : base(dbContext) { }

        public new void Update(User item)
        {
            var user = EPlastDBContext.Users.Find(item.Id);
            user.FirstName = item.FirstName;
            user.LastName = item.LastName;
            user.FatherName = item.FatherName;
            user.ImagePath = item.ImagePath;
            user.PhoneNumber = item.PhoneNumber;
            user.Comment = item.Comment;
            EPlastDBContext.Users.Update(user);
        }
    }
}
