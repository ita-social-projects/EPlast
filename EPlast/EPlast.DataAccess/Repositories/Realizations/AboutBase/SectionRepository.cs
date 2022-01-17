using System;
using System.Collections.Generic;
using System.Text;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;

namespace EPlast.DataAccess.Repositories
{
    public class SectionRepository: RepositoryBase<Section>, ISectionRepository
    {
        public SectionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
