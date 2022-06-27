using System.Collections.Generic;
using System.Linq;
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
            searchData = searchData?.ToLower();

            var found = EPlastDBContext.Set<UserRenewal>()
                .Include(ur => ur.User)
                .Include(ur => ur.City)
                .ThenInclude(city => city.Region)
                .Where(ur =>
                    string.IsNullOrWhiteSpace(searchData)
                    || ur.User.FirstName.ToLower().Contains(searchData)
                    || ur.User.LastName.ToLower().Contains(searchData)
                    || ur.City.Name.ToLower().Contains(searchData)
                    || ur.City.Region.RegionName.ToLower().Contains(searchData)
                    || ur.RequestDate.ToString().Contains(searchData)
                    || ur.User.Email.Contains(searchData)
                );

            var selected = found
                .Select(ur => new UserRenewalsTableObject
                {
                    Id = ur.Id,
                    UserName = ur.User.FirstName + ur.User.LastName,
                    UserId = ur.User.Id,
                    Approved = ur.Approved,
                    CityName = ur.City.Name,
                    CityId = ur.CityId,
                    Email = ur.User.Email,
                    RegionName = ur.City.Region.RegionName,
                    RequestDate = ur.RequestDate,
                    Subtotal = found.Count(),
                    Total = EPlastDBContext.Set<UserRenewal>().Count()
                });

            var items = selected
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToList();

            return items;
        }
    }
}
