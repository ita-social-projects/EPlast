using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class UserPrecautionRepository : RepositoryBase<UserPrecaution>, IUserPrecautionRepository
    {
        public UserPrecautionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {
        }
        
    }
}
