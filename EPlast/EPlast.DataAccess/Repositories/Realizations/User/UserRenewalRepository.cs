using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories.Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories.Realizations.User
{
    public class UserRenewalRepository : RepositoryBase<UserRenewal>, IUserRenewalRepository
    {
        public UserRenewalRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserRenewalsTableObject> GetUserRenewals(string searchData, int page, int pageSize)
        {
            return EPlastDBContext.Set<UserRenewalsTableObject>().FromSqlRaw(
                "dbo.usp_GetUserRenewals @searchData = {0}, @PageIndex = {1}, @PageSize = {2} ", 
                searchData, page, pageSize);
        }
    }
}
