using EPlast.DataAccess.Entities.EducatorsStaff;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class KVsRepository: RepositoryBase<KVs>, IKVsRepository
    {
        public KVsRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }
    }
}
