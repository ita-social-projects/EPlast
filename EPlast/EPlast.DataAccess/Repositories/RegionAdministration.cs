using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class RegionAdministrationRepository : RepositoryBase<RegionAdministration>, IRegionAdministrationRepository
    {
        public RegionAdministrationRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
