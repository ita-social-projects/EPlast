using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Blank;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories.Realizations.Blank
{
    public class UserCourseRepository : RepositoryBase<UserCourse>, IUserCourseRepository
    {

        public UserCourseRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
