using System;
using System.Collections.Generic;
using System.Text;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class TermsRepository: RepositoryBase<Terms>, ITermsRepository
    {
        public TermsRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}