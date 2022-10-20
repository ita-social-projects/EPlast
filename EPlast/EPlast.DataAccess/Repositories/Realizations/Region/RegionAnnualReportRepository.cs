#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories.Realizations.Region
{
    public class RegionAnnualReportRepository : RepositoryBase<RegionAnnualReport>, IRegionAnnualReportsRepository
    {
        public RegionAnnualReportRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {

        }

        public async Task<IEnumerable<RegionAnnualReportTableObject>> GetRegionAnnualReportsAsync(
            string userId,
            bool isAdmin,
            string? searchdata,
            int page,
            int pageSize,
            int sortKey,
            bool auth)
        {
            searchdata = searchdata?.ToLower();

            var regionsThatUserCanmanage = EPlastDBContext.Set<Entities.Region>()
                .Include(r => r.RegionAdministration)
                    .ThenInclude(ra => ra.AdminType)
                .Where(r => isAdmin || r.RegionAdministration.Any(ra =>
                    ra.UserId == userId
                    && (ra.AdminType.AdminTypeName == Roles.OkrugaHead ||
                         ra.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy)
                    && (ra.EndDate == null || ra.EndDate >= DateTime.Now)
                ))
                .Select(r => r.ID);

            var found = EPlastDBContext.Set<RegionAnnualReport>()
                .Include(rar => rar.Region)
                .Where(rar => isAdmin || !auth || regionsThatUserCanmanage.Contains(rar.RegionId))
                .Where(rar =>
                    string.IsNullOrWhiteSpace(searchdata)
                    || ("Непідтверджений".ToLower().Contains(searchdata) && rar.Status == AnnualReportStatus.Unconfirmed)
                    || ("Підтверджений".ToLower().Contains(searchdata) && rar.Status == AnnualReportStatus.Confirmed)
                    || ("Збережений".ToLower().Contains(searchdata) && rar.Status == AnnualReportStatus.Saved)
                    || rar.ID.ToString().Contains(searchdata)
                    || rar.RegionName.ToLower().Contains(searchdata)
                    || rar.Date.ToString().Contains(searchdata)
                );

            var selected = found
                .Select(rar => new RegionAnnualReportTableObject()
                {
                    Id = rar.ID,
                    RegionId = rar.RegionId,
                    RegionName = rar.RegionName,
                    Date = rar.Date,
                    Status = (int)rar.Status,
                    Count = found.Count(),
                    Total = EPlastDBContext.Set<RegionAnnualReport>().Count(),
                    CanManage = isAdmin || regionsThatUserCanmanage.Contains(rar.RegionId)
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
                        selected = selected.OrderBy(o => o.RegionName);
                        break;
                    }
                case -2:
                    {
                        selected = selected.OrderByDescending(o => o.RegionName);
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

        public async Task<IEnumerable<RegionMembersInfoTableObject>> GetRegionMembersInfoAsync(
            int regionId,
            int year,
            bool? getGeneral,
            int? page,
            int? pageSize)
        {
            var found = EPlastDBContext.Set<AnnualReport>()
                .Where(ar => ar.Date.Year == year && ar.City.RegionId == regionId);

            if (getGeneral == true)
            {
                var selected = await found
                    .Where(ar => ar.Status == AnnualReportStatus.Confirmed || ar.Status == AnnualReportStatus.Saved)
                    .Select(ar => new RegionMembersInfoTableObject()
                    {
                        NumberOfSeatsPtashat = ar.NumberOfSeatsPtashat,
                        NumberOfIndependentRiy = ar.NumberOfIndependentRiy,
                        NumberOfClubs = ar.NumberOfClubs,
                        NumberOfIndependentGroups = ar.NumberOfIndependentGroups,
                        NumberOfTeachers = ar.NumberOfTeachers,
                        NumberOfAdministrators = ar.NumberOfAdministrators,
                        NumberOfTeacherAdministrators = ar.NumberOfTeacherAdministrators,
                        NumberOfBeneficiaries = ar.NumberOfBeneficiaries,
                        NumberOfPlastpryiatMembers = ar.NumberOfPlastpryiatMembers,
                        NumberOfHonoraryMembers = ar.NumberOfHonoraryMembers,
                        NumberOfPtashata = ar.MembersStatistic.NumberOfPtashata,
                        NumberOfNovatstva = ar.MembersStatistic.NumberOfNovatstva,
                        NumberOfUnatstvaNoname = ar.MembersStatistic.NumberOfUnatstvaNoname,
                        NumberOfUnatstvaSupporters = ar.MembersStatistic.NumberOfUnatstvaSupporters,
                        NumberOfUnatstvaMembers = ar.MembersStatistic.NumberOfUnatstvaMembers,
                        NumberOfUnatstvaProspectors = ar.MembersStatistic.NumberOfUnatstvaProspectors,
                        NumberOfUnatstvaSkobVirlyts = ar.MembersStatistic.NumberOfUnatstvaSkobVirlyts,
                        NumberOfSeniorPlastynSupporters = ar.MembersStatistic.NumberOfSeniorPlastynSupporters,
                        NumberOfSeniorPlastynMembers = ar.MembersStatistic.NumberOfSeniorPlastynMembers,
                        NumberOfSeigneurSupporters = ar.MembersStatistic.NumberOfSeigneurSupporters,
                        NumberOfSeigneurMembers = ar.MembersStatistic.NumberOfSeigneurMembers
                    })
                    .ToListAsync();

                // @tvardero: I was unable to include Sum() inside LINQ Query, 
                // Using grouping and `group => group.Sum()` doesn't work well with included entities (LEFT JOIN MembersStatistic)
                var result = new RegionMembersInfoTableObject()
                {
                    Total = -1,
                    CityAnnualReportId = -1,
                    CityId = -1,
                    CityName = "Загалом",
                    ReportStatus = -1,
                    NumberOfSeatsPtashat = selected.Sum(o => o.NumberOfSeatsPtashat),
                    NumberOfIndependentRiy = selected.Sum(o => o.NumberOfIndependentRiy),
                    NumberOfClubs = selected.Sum(o => o.NumberOfClubs),
                    NumberOfIndependentGroups = selected.Sum(o => o.NumberOfIndependentGroups),
                    NumberOfTeachers = selected.Sum(o => o.NumberOfTeachers),
                    NumberOfAdministrators = selected.Sum(o => o.NumberOfAdministrators),
                    NumberOfTeacherAdministrators = selected.Sum(o => o.NumberOfTeacherAdministrators),
                    NumberOfBeneficiaries = selected.Sum(o => o.NumberOfBeneficiaries),
                    NumberOfPlastpryiatMembers = selected.Sum(o => o.NumberOfPlastpryiatMembers),
                    NumberOfHonoraryMembers = selected.Sum(o => o.NumberOfHonoraryMembers),
                    NumberOfPtashata = selected.Sum(o => o.NumberOfPtashata),
                    NumberOfNovatstva = selected.Sum(o => o.NumberOfNovatstva),
                    NumberOfUnatstvaNoname = selected.Sum(o => o.NumberOfUnatstvaNoname),
                    NumberOfUnatstvaSupporters = selected.Sum(o => o.NumberOfUnatstvaSupporters),
                    NumberOfUnatstvaMembers = selected.Sum(o => o.NumberOfUnatstvaMembers),
                    NumberOfUnatstvaProspectors = selected.Sum(o => o.NumberOfUnatstvaProspectors),
                    NumberOfUnatstvaSkobVirlyts = selected.Sum(o => o.NumberOfUnatstvaSkobVirlyts),
                    NumberOfSeniorPlastynSupporters = selected.Sum(o => o.NumberOfSeniorPlastynSupporters),
                    NumberOfSeniorPlastynMembers = selected.Sum(o => o.NumberOfSeniorPlastynMembers),
                    NumberOfSeigneurSupporters = selected.Sum(o => o.NumberOfSeigneurSupporters),
                    NumberOfSeigneurMembers = selected.Sum(o => o.NumberOfSeigneurMembers)
                };

                return new List<RegionMembersInfoTableObject>() { result };
            }
            else
            {
                var total = await found.CountAsync();

                var selected = found
                    .Select(ar => new RegionMembersInfoTableObject()
                    {
                        Total = total,
                        CityAnnualReportId = ar.ID,
                        CityId = ar.CityId,
                        CityName = ar.City.Name,
                        ReportStatus = (int?)ar.Status,
                        NumberOfSeatsPtashat = ar.NumberOfSeatsPtashat,
                        NumberOfIndependentRiy = ar.NumberOfIndependentRiy,
                        NumberOfClubs = ar.NumberOfClubs,
                        NumberOfIndependentGroups = ar.NumberOfIndependentGroups,
                        NumberOfTeachers = ar.NumberOfTeachers,
                        NumberOfAdministrators = ar.NumberOfAdministrators,
                        NumberOfTeacherAdministrators = ar.NumberOfTeacherAdministrators,
                        NumberOfBeneficiaries = ar.NumberOfBeneficiaries,
                        NumberOfPlastpryiatMembers = ar.NumberOfPlastpryiatMembers,
                        NumberOfHonoraryMembers = ar.NumberOfHonoraryMembers,
                        NumberOfPtashata = ar.MembersStatistic.NumberOfPtashata,
                        NumberOfNovatstva = ar.MembersStatistic.NumberOfNovatstva,
                        NumberOfUnatstvaNoname = ar.MembersStatistic.NumberOfUnatstvaNoname,
                        NumberOfUnatstvaSupporters = ar.MembersStatistic.NumberOfUnatstvaSupporters,
                        NumberOfUnatstvaMembers = ar.MembersStatistic.NumberOfUnatstvaMembers,
                        NumberOfUnatstvaProspectors = ar.MembersStatistic.NumberOfUnatstvaProspectors,
                        NumberOfUnatstvaSkobVirlyts = ar.MembersStatistic.NumberOfUnatstvaSkobVirlyts,
                        NumberOfSeniorPlastynSupporters = ar.MembersStatistic.NumberOfSeniorPlastynSupporters,
                        NumberOfSeniorPlastynMembers = ar.MembersStatistic.NumberOfSeniorPlastynMembers,
                        NumberOfSeigneurSupporters = ar.MembersStatistic.NumberOfSeigneurSupporters,
                        NumberOfSeigneurMembers = ar.MembersStatistic.NumberOfSeigneurMembers
                    });

                if (page > 0 && pageSize > 0)
                {
                    selected = selected
                        .Skip((int)(pageSize * (page - 1)))
                        .Take((int)pageSize);
                }

                return await selected.ToListAsync();
            }
        }
    }
}
