using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public interface IUserFormerMembershipDatesRepository : IRepositoryBase<UserFormerMembershipDates>
    {
        public Tuple<IEnumerable<UserFormerMembershipTable>, int> GetUserTableObjects(string userId);
    }
}
