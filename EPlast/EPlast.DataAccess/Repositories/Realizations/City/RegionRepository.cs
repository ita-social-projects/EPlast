using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
