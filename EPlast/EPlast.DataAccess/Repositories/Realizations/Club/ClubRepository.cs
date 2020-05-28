using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ClubRepository : RepositoryBase<Club>, IClubRepository
    {
        public ClubRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    
    }
}
