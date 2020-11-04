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
        public async Task<RegionAnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id)
        {
            DataAccess.Entities.Region region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                     predicate: a => a.ID == id,
                     include: source => source
                         .Include(a => a.Cities)
                         .ThenInclude(a => a.AnnualReports)
                         .Include(af => af.Cities)
                         .ThenInclude(af => af.AnnualReports)
                         .ThenInclude(af => af.MembersStatistic)
                         );
            var regionAnnualReport = new RegionAnnualReport()
            {
                NumberOfSeigneurMembers = region.Cities.Aggregate(0, (x, y) =>
                           x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfSeigneurMembers)),

                NumberOfSeigneurSupporters = region.Cities.Aggregate(0, (x, y) =>
                          x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfSeigneurSupporters)),

                NumberOfSeniorPlastynMembers = region.Cities.Aggregate(0, (x, y) =>
                         x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfSeniorPlastynMembers)),

                NumberOfSeniorPlastynSupporters = region.Cities.Aggregate(0, (x, y) =>
                        x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfSeniorPlastynSupporters)),

                NumberOfUnatstvaSkobVirlyts = region.Cities.Aggregate(0, (x, y) =>
                       x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfUnatstvaSkobVirlyts)),

                NumberOfUnatstvaProspectors = region.Cities.Aggregate(0, (x, y) =>
                      x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfUnatstvaProspectors)),

                NumberOfUnatstvaMembers = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfUnatstvaSupporters)),

                NumberOfUnatstvaSupporters = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfUnatstvaSupporters)),

                NumberOfUnatstvaNoname = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfUnatstvaNoname)),

                NumberOfNovatstva = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfNovatstva)),

                NumberOfPtashata = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.MembersStatistic.NumberOfPtashata)),

                NumberOfSeatsPtashat = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfSeatsPtashat)),

                NumberOfIndependentRiy = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfIndependentRiy)),

                NumberOfClubs = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfClubs)),

                NumberOfIndependentGroups = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfIndependentGroups)),

                NumberOfPlastpryiatMembers = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfPlastpryiatMembers)),

                NumberOfBeneficiaries = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfBeneficiaries)),

                NumberOfHonoraryMembers = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfHonoraryMembers)),

                NumberOfAdministrators = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfAdministrators)),

                NumberOfTeacherAdministrators = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfTeacherAdministrators)),

                NumberOfTeachers = region.Cities.Aggregate(0, (x, y) =>
                    x += y.AnnualReports.Aggregate(0, (z, p) => z += p.NumberOfTeachers)),
            };

            return await _regionAccessService.HasAccessAsync(claimsPrincipal, regionAnnualReport.ID) ? _mapper.Map<RegionAnnualReport, RegionAnnualReportDTO>(regionAnnualReport)
                : throw new UnauthorizedAccessException();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<RegionAnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync(include:
                 source => source
                        .Include(a => a.Region));
            var citiesDTO = await _regionAccessService.GetRegionsAsync(claimsPrincipal);
            var filteredAnnualReports = annualReports.Where(a => citiesDTO.Any(c => c.ID == a.Region.ID));
            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(annualReports);
        }

        public async Task CreateAsync(ClaimsPrincipal claimsPrincipal, RegionAnnualReportDTO regionAnnualReportDTO)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                predicate: a => a.ID == regionAnnualReportDTO.Region.ID);
            if (await CheckCreated(region.ID))
            {
                throw new InvalidOperationException();
            }
            if (!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException();
            }
            var regionAnnualReport = _mapper.Map<RegionAnnualReportDTO, RegionAnnualReport>(regionAnnualReportDTO);
            regionAnnualReport.Date = DateTime.Now;
            await _repositoryWrapper.RegionAnnualReports.CreateAsync(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        private async Task<bool> CheckCreated(int Id)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.Region.ID == Id && a.Date.Year == DateTime.Now.Year) != null;
        }
    }
}
