using EPlast.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public new void Update(User item)
        {
            var user = EPlastDBContext.Users.Find(item.Id);
            user.FirstName = item.FirstName;
            user.LastName = item.LastName;
            user.FatherName = item.FatherName;
            user.ImagePath = item.ImagePath;
            user.PhoneNumber = item.PhoneNumber;
            EPlastDBContext.Users.Update(user);
        }

        public IEnumerable<UserTableObject> GetUserTableObjects(int pageNum, int pageSize)
        {
            return EPlastDBContext.Set<UserTableObject>().FromSqlRaw("dbo.usp_GetUserInfo @PageIndex = {0}, @PageSize = {1}", pageNum, pageSize);
        }
    }
}
