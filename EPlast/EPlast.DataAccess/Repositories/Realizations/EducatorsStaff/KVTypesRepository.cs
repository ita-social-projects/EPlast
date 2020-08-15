using EPlast.DataAccess.Entities.EducatorsStaff;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories.Realizations.EducatorsStaff
{
    public class KVTypesRepository: RepositoryBase<KVTypes>, IKVTypesRepository
    {
        public KVTypesRepository(EPlastDBContext dbContext)
       : base(dbContext)
        {
        }

    }
}
