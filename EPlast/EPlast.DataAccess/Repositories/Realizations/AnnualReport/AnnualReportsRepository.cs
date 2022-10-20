#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportsRepository : RepositoryBase<AnnualReport>, IAnnualReportsRepository
    {
        public AnnualReportsRepository(EPlastDBContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<AnnualReportTableObject>> GetAnnualReportsAsync(
            string userId,
            bool isAdmin,
            string? searchdata,
            int page,
            int pageSize,
            int sortKey,
            bool auth
        )
        {
            searchdata = searchdata?.ToLower();

            var citiesThatСityAdminCanManage = EPlastDBContext.Set<City>()
                .Include(c => c.CityAdministration)
                    .ThenInclude(ca => ca.AdminType)
                .Where(c => isAdmin || c.CityAdministration.Any(ca =>
                    ca.UserId == userId
                    && (ca.AdminType.AdminTypeName == Roles.CityHead ||
                       ca.AdminType.AdminTypeName == Roles.CityHeadDeputy)
                    && (ca.EndDate == null || ca.EndDate >= DateTime.Now)
                ))
                .Select(c => c.ID);

            var citiesThatRegionAdminCanManage = EPlastDBContext.Set<City>()
               .Include(r => r.Region)
                  .ThenInclude(ra => ra.RegionAdministration)
                  .ThenInclude(ca => ca.AdminType)
               .Where(ra => isAdmin ||
                  ra.Region.RegionAdministration.Any(ca => ca.UserId == userId
                   && (ca.AdminType.AdminTypeName == Roles.OkrugaHead ||
                    ca.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy)
                   && (ca.EndDate == null  || ca.EndDate >= DateTime.Now)
                   ))
               .Select(c => c.ID);

            var found = EPlastDBContext.Set<AnnualReport>()
                .Include(ar => ar.City)
                    .ThenInclude(c => c.Region)
                .Where(ar => isAdmin || !auth || citiesThatСityAdminCanManage.Contains(ar.CityId)
                        || citiesThatRegionAdminCanManage.Contains(ar.CityId))
                .Where(ar =>
                    string.IsNullOrWhiteSpace(searchdata)
                    || ("Непідтверджений".ToLower().Contains(searchdata) && ar.Status == AnnualReportStatus.Unconfirmed)
                    || ("Підтверджений".ToLower().Contains(searchdata) && ar.Status == AnnualReportStatus.Confirmed)
                    || ("Збережений".ToLower().Contains(searchdata) && ar.Status == AnnualReportStatus.Saved)
                    || ar.ID.ToString().Contains(searchdata)
                    || ar.City.Name.ToLower().Contains(searchdata)
                    || ar.City.Region.RegionName.ToLower().Contains(searchdata)
                    || ar.Date.ToString().Contains(searchdata)
                );

            var selected = found
                .Select(a => new AnnualReportTableObject()
                {
                    Id = a.ID,
                    CityId = a.CityId,
                    CityName = a.City.Name,
                    RegionName = a.City.Region.RegionName,
                    Date = a.Date,
                    Status = (int)a.Status,
                    Count = found.Count(),
                    Total = EPlastDBContext.Set<AnnualReport>().Count(),
                    CanManage = isAdmin || citiesThatСityAdminCanManage.Contains(a.CityId)
                        || citiesThatRegionAdminCanManage.Contains(a.CityId)
                });

            switch (sortKey)
            {
                case 1:
                    {
                        selected = selected.OrderBy(a => a.Id);
                        break;
                    }
                case -1:
                    {
                        selected = selected.OrderByDescending(a => a.Id);
                        break;
                    }
                case 2:
                    {
                        selected = selected.OrderBy(a => a.CityName);
                        break;
                    }
                case -2:
                    {
                        selected = selected.OrderByDescending(a => a.CityName);
                        break;
                    }
                case 3:
                    {
                        selected = selected.OrderBy(a => a.RegionName);
                        break;
                    }
                case -3:
                    {
                        selected = selected.OrderByDescending(a => a.RegionName);
                        break;
                    }
                case 4:
                    {
                        selected = selected.OrderBy(a => a.Date);
                        break;
                    }
                case -4:
                    {
                        selected = selected.OrderByDescending(a => a.Date);
                        break;
                    }
                default:
                    {
                        selected = selected.OrderBy(a => a.CanManage).ThenBy(a => a.Id);
                        break;
                    }
            }

            var items = await selected
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return items;
        }
    }
}
