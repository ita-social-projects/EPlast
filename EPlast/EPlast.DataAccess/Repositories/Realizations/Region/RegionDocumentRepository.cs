using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories.Realizations.Region
{
    public class RegionDocumentRepository:RepositoryBase<RegionDocuments>, IRegionDocumentRepository
    {
            public RegionDocumentRepository(EPlastDBContext dbContext)
                : base(dbContext)
            {
            }
        
    }
}
