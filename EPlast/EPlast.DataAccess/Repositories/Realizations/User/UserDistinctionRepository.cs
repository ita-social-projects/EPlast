using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class UserDistinctionRepository : RepositoryBase<UserDistinction>, IUserDistinctionRepository
    {
        public UserDistinctionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {
        }

    }
}
