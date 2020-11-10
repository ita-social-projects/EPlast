using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Region
{
    public class RegionAnnualReportService : IRegionAnnualReportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IRegionAccessService _regionAccessService;
        private readonly IMapper _mapper;

        public RegionAnnualReportService(IRepositoryWrapper repositoryWrapper,
            IRegionAccessService regionAccessService, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _regionAccessService = regionAccessService;
            _mapper = mapper;
        }

        ///<inheritdoc/>
        public async Task<RegionAnnualReportDTO> CreateByNameAsync(ClaimsPrincipal claimsPrincipal, int id, int year)
        {  
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(a => a.ID == id);

            if(!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException("User doesn't have access");
            }
            if (await CheckCreated(id,year))
            {
                throw new InvalidOperationException("Report is already crated!");
            }
            var cities = await _repositoryWrapper.City.GetAllAsync(
                predicate: a => a.RegionId == id
                );

            var AnnualReports = new List<AnnualReport>();
            var MembersStatistics = new List<MembersStatistic>();

            foreach(var city in cities)
            {
                var reports = await _repositoryWrapper.AnnualReports.GetAllAsync(a => a.Date.Year == year && a.CityId == city.ID);
                if (reports.Count() > 0)
                {
                    foreach(var report in reports)
                    {
                        AnnualReports.Add(report);
                    }
                }
            }

            foreach(var report in AnnualReports)
            {
                var statistics = await _repositoryWrapper.MembersStatistics.GetAllAsync(a => a.AnnualReportId == report.ID);
                if (statistics.Count() > 0)
                {
                    foreach (var stat in statistics)
                    {
                        MembersStatistics.Add(stat);
                    }
                }
            }

            if (cities != null)
            {
                var regionAnnualReport = new RegionAnnualReport()
                {
                    RegionName = region.RegionName,

                    RegionId = id,

                    Date = DateTime.Now,

                    NumberOfSeigneurMembers = MembersStatistics.Select(x=>x.NumberOfSeigneurMembers).Sum(),

                    NumberOfSeigneurSupporters = MembersStatistics.Select(x => x.NumberOfSeigneurSupporters).Sum(),

                    NumberOfSeniorPlastynMembers = MembersStatistics.Select(x => x.NumberOfSeniorPlastynMembers).Sum(),

                    NumberOfSeniorPlastynSupporters = MembersStatistics.Select(x => x.NumberOfSeniorPlastynSupporters).Sum(),

                    NumberOfUnatstvaSkobVirlyts = MembersStatistics.Select(x => x.NumberOfUnatstvaSkobVirlyts).Sum(),

                    NumberOfUnatstvaProspectors = MembersStatistics.Select(x => x.NumberOfUnatstvaProspectors).Sum(),

                    NumberOfUnatstvaMembers = MembersStatistics.Select(x => x.NumberOfUnatstvaMembers).Sum(),

                    NumberOfUnatstvaSupporters = MembersStatistics.Select(x => x.NumberOfUnatstvaSupporters).Sum(),

                    NumberOfUnatstvaNoname = MembersStatistics.Select(x => x.NumberOfUnatstvaNoname).Sum(),

                    NumberOfNovatstva = MembersStatistics.Select(x => x.NumberOfNovatstva).Sum(),

                    NumberOfPtashata = MembersStatistics.Select(x => x.NumberOfPtashata).Sum(),

                    NumberOfSeatsPtashat= AnnualReports.Select(x => x.NumberOfSeatsPtashat).Sum(),

                    NumberOfIndependentRiy = AnnualReports.Select(x => x.NumberOfIndependentRiy).Sum(),

                    NumberOfClubs = AnnualReports.Select(x => x.NumberOfClubs).Sum(),

                    NumberOfIndependentGroups = AnnualReports.Select(x => x.NumberOfIndependentGroups).Sum(),

                    NumberOfPlastpryiatMembers = AnnualReports.Select(x => x.NumberOfPlastpryiatMembers).Sum(),

                    NumberOfBeneficiaries = AnnualReports.Select(x => x.NumberOfBeneficiaries).Sum(),

                    NumberOfHonoraryMembers = AnnualReports.Select(x => x.NumberOfHonoraryMembers).Sum(),

                    NumberOfAdministrators = AnnualReports.Select(x => x.NumberOfAdministrators).Sum(),

                    NumberOfTeacherAdministrators = AnnualReports.Select(x => x.NumberOfTeacherAdministrators).Sum(),

                    NumberOfTeachers = AnnualReports.Select(x => x.NumberOfTeachers).Sum(),
                };

                _repositoryWrapper.RegionAnnualReports.Create(regionAnnualReport);
                await _repositoryWrapper.SaveAsync();

                return _mapper.Map<RegionAnnualReportDTO>(regionAnnualReport);
            }
            else
            {
                throw new InvalidOperationException("Can't be empty report!");
            } 
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<RegionAnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync(include:
                 source => source
                        .Include(a => a.Region));
            var citiesDTO = await _regionAccessService.GetRegionsAsync(claimsPrincipal);
            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(annualReports);
        }

        public async Task CreateAsync(ClaimsPrincipal claimsPrincipal, RegionAnnualReportDTO regionAnnualReportDTO)
        {
            var region = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionAnnualReportDTO.RegionId&&a.Date.Year==regionAnnualReportDTO.Date.Year);
            if (await CheckCreated(region.ID,region.Date.Year))
            {
                throw new InvalidOperationException("Report is already crated!");
            }
            if (!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException("No access for creating Annual Report!");
            }
            var regionAnnualReport = _mapper.Map<RegionAnnualReportDTO, RegionAnnualReport>(regionAnnualReportDTO);
            regionAnnualReport.Date = DateTime.Now;
            await _repositoryWrapper.RegionAnnualReports.CreateAsync(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        private async Task<bool> CheckCreated(int regionId, int year)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId== regionId && a.Date.Year == year) != null;
        }
        
        public async Task<RegionAnnualReportDTO> GetReportByIdAsync(int id, int year)
        {
            return _mapper.Map<RegionAnnualReport, RegionAnnualReportDTO>(await _repositoryWrapper.RegionAnnualReports.GetFirstAsync(predicate: i => i.RegionId == id && i.Date.Year == year));
        }

        public async Task<IEnumerable<RegionAnnualReportDTO>> GetAllRegionsReportsAsync()
        {
            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(await _repositoryWrapper.RegionAnnualReports.FindAll().ToListAsync());
        }

    }
}
