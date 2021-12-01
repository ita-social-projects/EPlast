using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.DataAccess.Repositories.Interfaces.User
{
    public interface IUserRenewalRepository: IRepositoryBase<UserRenewal>
    {
        IEnumerable<UserRenewalsTableObject> GetUserRenewals(string searchData, int page, int pageSize);
    }
}
