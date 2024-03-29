﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(
            int pageNum,
            int pageSize,
            string tab,
            string regions,
            string cities,
            string clubs,
            string degrees,
            int sortKey,
            string searchData,
            string filterRoles = "",
            string filterKadras = "",
            string andClubs = null)
        {
            var items = EPlastDBContext.Set<User>()
                .Include(x => x.UserProfile)
                .Include(x => x.CityMembers)
                .Include(x => x.ClubMembers)
                .Include(x => x.UserPlastDegrees)
                .Include(x => x.UserMembershipDates)
                .Include(x => x.UsersKadras)
                .ThenInclude(x => x.KadraVykhovnykivType)
                .Select(x => new UserTableObject()
                {
                    ID = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.LastName + " " + x.FirstName,
                    Birthday = x.UserProfile.Birthday,
                    Entry = x.UserMembershipDates.FirstOrDefault(umd => umd.UserId == x.Id).DateEntry,
                    Membership = x.UserMembershipDates.FirstOrDefault(umd => umd.UserId == x.Id).DateMembership,
                    Kadra = string.Join(", ", x.UsersKadras.Where(uk => uk.UserId == x.Id).Select(y => y.KadraVykhovnykivType.Name)),
                    Gender = x.UserProfile.Gender.Name,
                    RegionName = EPlastDBContext.Set<Region>().FirstOrDefault(y => y.ID == x.RegionId).RegionName
                        ?? x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).City.Region.RegionName,
                    CityName = x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).City.Name,
                    ClubName = x.ClubMembers.FirstOrDefault(y => y.UserId == x.Id).Club.Name,
                    Address = x.UserProfile.Address,
                    PhoneNumber = x.PhoneNumber,
                    Referal = x.UserProfile.Referal,
                    Oblast = x.UserProfile.Oblast,
                    PlastDegree = x.UserPlastDegrees.PlastDegree.Name,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed,
                    UPUDegree = x.UserProfile.UpuDegree.Name,
                    UserSystemId = x.UserProfile.ID,
                    RegionId = x.CityMembers.FirstOrDefault(y => y.UserId == x.Id) != null ? x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).City.Region.ID : x.RegionId,
                    CityId = x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).City.ID,
                    ClubId = x.ClubMembers.FirstOrDefault(y => y.UserId == x.Id).Club.ID,
                    DegreeId = x.UserPlastDegrees.PlastDegree.Id,
                    Roles = string.Join(", ", EPlastDBContext.Roles
                       .Where(r => (EPlastDBContext.UserRoles
                       .Where(y => y.UserId == x.Id)
                       .Select(y => y.RoleId))
                       .Contains(r.Id))),
                    Comment = x.Comment,
                    IsCityFollower = x.CityMembers.FirstOrDefault(y => y.UserId == x.Id) != null ? !x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).IsApproved : true,
                    IsClubFollower = x.ClubMembers.FirstOrDefault(y => y.UserId == x.Id) != null ? !x.ClubMembers.FirstOrDefault(y => y.UserId == x.Id).IsApproved : false,
                });

            //tab sorting
            items = tab switch
            {
                "confirmed" => items.Where(r => r.EmailConfirmed),
                "registered" => items.Where(r => r.EmailConfirmed && (r.IsCityFollower || r.IsClubFollower)),
                "unconfirmed" => items.Where(r => !r.EmailConfirmed),
                _ => items.Where(r => r.EmailConfirmed)
            };
            
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
                items = items.Where(r => string.IsNullOrWhiteSpace(searchData)
                    || r.FirstName.ToLower().Contains(searchData)
                    || r.LastName.ToLower().Contains(searchData)
                    || (r.FirstName.ToLower() + " " + r.LastName.ToLower()).Contains(searchData)
                    || r.RegionName.ToLower().Contains(searchData)
                    || r.CityName.ToLower().Contains(searchData)
                    || r.ClubName.ToLower().Contains(searchData)
                    || r.PlastDegree.ToLower().Contains(searchData)
                    || r.UPUDegree.ToLower().Contains(searchData)
                    || r.Email.ToLower().Contains(searchData)
                    || r.Comment.ToLower().Contains(searchData)
                    );
            }

            IEnumerable<UserTableObject> finalItems = await items.ToListAsync();

            //roles 
            if (!string.IsNullOrEmpty(filterRoles))
            {
                var frs = filterRoles.Split(",");
                foreach (var fr in frs)
                {
                    var filter = fr.Trim();
                    finalItems = finalItems.Where(r => r.Roles.Contains(filter));
                }
            }

            //kadras 
            if (!string.IsNullOrEmpty(filterKadras))
            {
                var fks = filterKadras.Split(",");
                foreach (var fr in fks)
                {
                    var filter = fr.Trim();
                    finalItems = finalItems.Where(r => r.Kadra.Contains(filter));
                }
            }

            // filter out super admins and former members of plast
            finalItems = finalItems.Where(u => !u.Roles.Contains(Roles.Admin));
            if (tab == "registered") finalItems = finalItems.Where(u => !u.Roles.Contains(Roles.FormerPlastMember));
            if (tab == "confirmed") finalItems = finalItems.Where(u => !u.Roles.Contains(Roles.FormerPlastMember));
            if (tab == "formers") finalItems = finalItems.Where(u => u.Roles.Contains(Roles.FormerPlastMember));

            int rowCount = finalItems.Count();

            //items ordering
            finalItems = SortItems(finalItems, sortKey);

            finalItems = finalItems
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);

            return new Tuple<IEnumerable<UserTableObject>, int>(finalItems, rowCount);
        }

        private IEnumerable<UserTableObject> SortItems(IEnumerable<UserTableObject> items, int sortKey)
        {
            switch (sortKey)
            {
                case 2:
                    items = items
                       .OrderBy(x => x.FirstName);
                    break;
                case -2:
                    items = items
                       .OrderByDescending(x => x.FirstName);
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
                case 10:
                    items = items
                       .OrderBy(x => x.Entry);
                    break;
                case -10:
                    items = items
                       .OrderByDescending(x => x.Entry);
                    break;
                case 11:
                    items = items
                       .OrderBy(x => x.Membership);
                    break;
                case -11:
                    items = items
                       .OrderByDescending(x => x.Membership);
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
