using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class AdminTypeRepository :RepositoryBase<AdminType>, IAdminTypeRepository
    {
        public AdminTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
