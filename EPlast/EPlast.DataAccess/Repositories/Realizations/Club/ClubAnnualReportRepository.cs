#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories.Realizations.Club
{
    public class ClubAnnualReportRepository: RepositoryBase<ClubAnnualReport>, IClubAnnualReportsRepository
    {
        public ClubAnnualReportRepository(EPlastDBContext dbContext)
            : base(dbContext)
        { }

        public async Task<IEnumerable<ClubAnnualReportTableObject>> GetClubAnnualReportsAsync(
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

            var clubsThatUserCanManage = EPlastDBContext.Set<Entities.Club>()
                .Include(c => c.ClubAdministration)
                    .ThenInclude(ca => ca.AdminType)
                .Where(c => isAdmin || c.ClubAdministration.Any(ca =>
                    ca.UserId == userId
                    && ca.AdminType.AdminTypeName == "Голова Куреня"
                    && (ca.EndDate == null || ca.EndDate >= DateTime.Now)
                ))
                .Select(c => c.ID);

            var found = EPlastDBContext.Set<ClubAnnualReport>()
                .Include(car => car.Club)
                .Where(car => isAdmin || !auth || clubsThatUserCanManage.Contains(car.ID))
                .Where(car =>
                    string.IsNullOrWhiteSpace(searchdata)
                    || ("Непідтверджений".ToLower().Contains(searchdata) && car.Status == AnnualReportStatus.Unconfirmed)
                    || ("Підтверджений".ToLower().Contains(searchdata) && car.Status == AnnualReportStatus.Confirmed)
                    || ("Збережений".ToLower().Contains(searchdata) && car.Status == AnnualReportStatus.Saved)
                    || car.ID.ToString().Contains(searchdata)
                    || car.Club.Name.ToLower().Contains(searchdata)
                    || car.Date.ToString().Contains(searchdata)
                );

            var selected = found
                .Select(car => new ClubAnnualReportTableObject()
                {
                    Id = car.ID,
                    ClubId = car.ClubId,
                    ClubName = car.Club.Name,
                    Date = car.Date,
                    Status = (int)car.Status,
                    Count = found.Count(),
                    Total = EPlastDBContext.Set<ClubAnnualReport>().Count(),
                    CanManage = isAdmin || clubsThatUserCanManage.Contains(car.ID)
                });

            switch (sortKey)
            {
                case 1:
                    {
                        selected = selected.OrderBy(o => o.Id);
                        break;
                    }
                case -1:
                    {
                        selected = selected.OrderByDescending(o => o.Id);
                        break;
                    }
                case 2:
                    {
                        selected = selected.OrderBy(o => o.ClubName);
                        break;
                    }
                case -2:
                    {
                        selected = selected.OrderByDescending(o => o.ClubName);
                        break;
                    }
                case 3:
                    {
                        selected = selected.OrderBy(o => o.Date);
                        break;
                    }
                case -3:
                    {
                        selected = selected.OrderByDescending(o => o.Date);
                        break;
                    }
                default:
                    {
                        selected = selected.OrderBy(o => o.CanManage).ThenBy(o => o.Id);
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
