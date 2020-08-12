using EPlast.DataAccess.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Text;

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
