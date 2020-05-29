using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ClubAdministrationRepository : RepositoryBase<ClubAdministration>, IClubAdministrationRepository
    {
        public ClubAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
