using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
