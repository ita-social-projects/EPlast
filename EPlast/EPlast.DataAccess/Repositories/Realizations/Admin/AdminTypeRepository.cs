using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public class AdminTypeRepository : RepositoryBase<AdminType>, IAdminTypeRepository
    {
        public AdminTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public Task<int> GetUsersCountAsync()
        {
            return EPlastDBContext.Users.CountAsync();
        }

        public async Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(int pageNum, int pageSize, string tab, string regions, string cities, string clubs, string degrees, int sortKey, string searchData, string filterRoles = "", string andClubs = null)
        {
            var items = EPlastDBContext.Set<User>()
                .Include(x => x.UserProfile)
                .Include(x => x.CityMembers)
                .Include(x => x.ClubMembers)
                .Include(x => x.UserPlastDegrees)
                .Select(x => new UserTableObject()
                {
                    ID = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Birthday = x.UserProfile.Birthday,
                    Gender = x.UserProfile.Gender.Name,
                    RegionName = x.CityMembers.Where(y => y.UserId == x.Id).FirstOrDefault().City.Region.RegionName,
                    CityName = x.CityMembers.Where(y => y.UserId == x.Id).FirstOrDefault().City.Name,
                    ClubName = x.ClubMembers.Where(y => y.UserId == x.Id).FirstOrDefault().Club.Name,
                    PlastDegree = x.UserPlastDegrees.PlastDegree.Name,
                    Email = x.Email,
                    UPUDegree = x.UserProfile.UpuDegree.Name,
                    UserSystemId = x.UserProfile.ID,
                    RegionId = x.CityMembers.Where(y => y.UserId == x.Id).FirstOrDefault().City.Region.ID,
                    CityId = x.CityMembers.Where(y => y.UserId == x.Id).FirstOrDefault().City.ID,
                    ClubId = x.ClubMembers.Where(y => y.UserId == x.Id).FirstOrDefault().Club.ID,
                    DegreeId = x.UserPlastDegrees.PlastDegree.Id,
                    Roles = string.Join(", ", EPlastDBContext.Roles
                       .Where(r => (EPlastDBContext.UserRoles
                       .Where(y => y.UserId == x.Id)
                       .Select(y => y.RoleId))
                       .Contains(r.Id)))
                });

            //region sorting
            if (!string.IsNullOrEmpty(regions))
            {
                if (regions == "-10")
                {
                    items = items.Where(r => r.RegionId != null);
                }
                else
                {
                    items = items.Where(r => regions.Split(",", StringSplitOptions.None).Contains(r.RegionId.ToString()));
                }
            }
            //city sorting
            if (!string.IsNullOrEmpty(cities))
            {
                if (cities == "-1")
                {
                    items = items.Where(r => r.CityId != null);
                }
                else
                {
                    items = items.Where(r => cities.Split(",", StringSplitOptions.None).Contains(r.CityId.ToString()));
                }
            }
            //club sorting
            if (!string.IsNullOrEmpty(clubs))
            {
                if (clubs == "-2")
                {
                    items = items.Where(r => r.ClubId != null);
                }
                else
                {
                    items = items.Where(r => clubs.Split(",", StringSplitOptions.None).Contains(r.ClubId.ToString()));
                }
            }
            //degrees sorting
            if (!string.IsNullOrEmpty(degrees))
            {
                if (degrees == "-3")
                {
                    items = items.Where(r => r.DegreeId != null);
                }
                else
                {
                    items = items.Where(r => degrees.Split(",", StringSplitOptions.None).Contains(r.DegreeId.ToString()));
                }
            }
            //search
            if (!string.IsNullOrWhiteSpace(searchData))
            {
                items = items.Where(r => string.IsNullOrWhiteSpace(searchData) ||
                r.FirstName.ToLower().Contains(searchData)
                    || r.RegionName.ToLower().Contains(searchData)
                    || r.CityName.ToLower().Contains(searchData)
                    || r.ClubName.ToLower().Contains(searchData)
                    || r.PlastDegree.ToLower().Contains(searchData)
                    || r.UPUDegree.ToLower().Contains(searchData)
                    || r.Email.ToLower().Contains(searchData)
                    );
            }

            IEnumerable<UserTableObject> finalItems = await items.ToListAsync();

            //roles sorting
            if (!string.IsNullOrEmpty(filterRoles))
            {
                var frs = filterRoles.Split(",");
                foreach (var fr in frs)
                {
                    var filter = fr.Trim();
                    finalItems = finalItems.Where(r => r.Roles.Contains(filter));
                }
            }

            int rowCount = finalItems.Count();

            //items ordering
            finalItems = sortItems(finalItems, sortKey);

            finalItems = finalItems
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);

            return new Tuple<IEnumerable<UserTableObject>, int>(finalItems, rowCount);
        }

        private IEnumerable<UserTableObject> sortItems(IEnumerable<UserTableObject> items, int sortKey)
        {
            switch (sortKey)
            {
                case -1:
                    items = items
                        .OrderByDescending(x => x.UserSystemId);
                    break;
                case 2:
                    items = items
                       .OrderBy(x => x.FirstName);
                    break;
                case -2:
                    items = items
                       .OrderBy(x => x.FirstName);
                    break;
                case 3:
                    items = items
                       .OrderBy(x => x.LastName);
                    break;
                case -3:
                    items = items
                       .OrderByDescending(x => x.LastName);
                    break;
                case 4:
                    items = items
                       .OrderBy(x => x.Birthday);
                    break;
                case -4:
                    items = items
                       .OrderByDescending(x => x.Birthday);
                    break;
                case 5:
                    items = items
                       .OrderBy(x => x.RegionName);
                    break;
                case -5:
                    items = items
                       .OrderByDescending(x => x.RegionName);
                    break;
                case 6:
                    items = items
                       .OrderBy(x => x.CityName);
                    break;
                case -6:
                    items = items
                       .OrderByDescending(x => x.CityName);
                    break;
                case 7:
                    items = items
                       .OrderBy(x => x.ClubName);
                    break;
                case -7:
                    items = items
                       .OrderByDescending(x => x.ClubName);
                    break;
                case 8:
                    items = items
                       .OrderBy(x => x.PlastDegree);
                    break;
                case -8:
                    items = items
                       .OrderByDescending(x => x.PlastDegree);
                    break;
                case 9:
                    items = items
                       .OrderBy(x => x.UPUDegree);
                    break;
                case -9:
                    items = items
                       .OrderByDescending(x => x.UPUDegree);
                    break;
                default:
                    items = items
                     .OrderBy(x => x.UserSystemId);
                    break;
            }
            return items;
        }
    }
}
