using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<RegionAnnualReportDTO> CreateByNameAsync(User claimsPrincipal, int id, int year,
            RegionAnnualReportQuestions regionAnnualReportQuestions)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(a => a.ID == id);

            if (!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException("User doesn't have access");
            }

            if (await CheckCreatedAsync(id, year))
            {
                throw new InvalidOperationException("Report is already created!");
            }

            var annualReports = (await _repositoryWrapper.AnnualReports.GetAllAsync(a => a.Date.Year == year && a.City.RegionId == id && a.Status == AnnualReportStatus.Confirmed))
                .Where(result => result != null).ToList();

            var membersStatistics = (await _repositoryWrapper.MembersStatistics.GetAllAsync(predicate: m => m.AnnualReport.City.RegionId == id))
                .Where(result => result != null).ToList();

            DateTime reportDate;

            if(year == DateTime.Now.Year)
            {
                reportDate = DateTime.Now;
            }
            else
            {
                reportDate = new DateTime(year, 12, 31);
            }

            var regionAnnualReport = new RegionAnnualReport()
            {
                RegionName = region.RegionName,

                RegionId = id,

                Date = reportDate,

                NumberOfSeigneurMembers = membersStatistics.Select(x => x.NumberOfSeigneurMembers).Sum(),

                NumberOfSeigneurSupporters = membersStatistics.Select(x => x.NumberOfSeigneurSupporters).Sum(),

                NumberOfSeniorPlastynMembers = membersStatistics.Select(x => x.NumberOfSeniorPlastynMembers).Sum(),

                NumberOfSeniorPlastynSupporters = membersStatistics.Select(x => x.NumberOfSeniorPlastynSupporters).Sum(),

                NumberOfUnatstvaSkobVirlyts = membersStatistics.Select(x => x.NumberOfUnatstvaSkobVirlyts).Sum(),

                NumberOfUnatstvaProspectors = membersStatistics.Select(x => x.NumberOfUnatstvaProspectors).Sum(),

                NumberOfUnatstvaMembers = membersStatistics.Select(x => x.NumberOfUnatstvaMembers).Sum(),

                NumberOfUnatstvaSupporters = membersStatistics.Select(x => x.NumberOfUnatstvaSupporters).Sum(),

                NumberOfUnatstvaNoname = membersStatistics.Select(x => x.NumberOfUnatstvaNoname).Sum(),

                NumberOfNovatstva = membersStatistics.Select(x => x.NumberOfNovatstva).Sum(),

                NumberOfPtashata = membersStatistics.Select(x => x.NumberOfPtashata).Sum(),

                NumberOfSeatsPtashat = annualReports.Select(x => x.NumberOfSeatsPtashat).Sum(),

                NumberOfIndependentRiy = annualReports.Select(x => x.NumberOfIndependentRiy).Sum(),

                NumberOfClubs = annualReports.Select(x => x.NumberOfClubs).Sum(),

                NumberOfIndependentGroups = annualReports.Select(x => x.NumberOfIndependentGroups).Sum(),

                NumberOfPlastpryiatMembers = annualReports.Select(x => x.NumberOfPlastpryiatMembers).Sum(),

                NumberOfBeneficiaries = annualReports.Select(x => x.NumberOfBeneficiaries).Sum(),

                NumberOfHonoraryMembers = annualReports.Select(x => x.NumberOfHonoraryMembers).Sum(),

                NumberOfAdministrators = annualReports.Select(x => x.NumberOfAdministrators).Sum(),

                NumberOfTeacherAdministrators = annualReports.Select(x => x.NumberOfTeacherAdministrators).Sum(),

                NumberOfTeachers = annualReports.Select(x => x.NumberOfTeachers).Sum(),

                StateOfPreparation = regionAnnualReportQuestions.StateOfPreparation,

                Characteristic = regionAnnualReportQuestions.Characteristic,

                ChurchCooperation = regionAnnualReportQuestions.ChurchCooperation,

                InvolvementOfVolunteers = regionAnnualReportQuestions.InvolvementOfVolunteers,

                ImportantNeeds = regionAnnualReportQuestions.ImportantNeeds,

                SocialProjects = regionAnnualReportQuestions.SocialProjects,

                StatusOfStrategy = regionAnnualReportQuestions.StatusOfStrategy,

                SuccessStories = regionAnnualReportQuestions.SuccessStories,

                ProblemSituations = regionAnnualReportQuestions.ProblemSituations,

                TrainedNeeds = regionAnnualReportQuestions.TrainedNeeds,

                PublicFunding = regionAnnualReportQuestions.PublicFunding,

                Fundraising = regionAnnualReportQuestions.Fundraising
            };

            _repositoryWrapper.RegionAnnualReports.Create(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<RegionAnnualReportDTO>(regionAnnualReport);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<RegionAnnualReportDTO>> GetAllAsync(User claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync(include:
                 source => source
                 .Include(a => a.Region));

            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(annualReports);
        }

        public async Task CreateAsync(User claimsPrincipal, RegionAnnualReportDTO regionAnnualReportDTO)
        {
            var region = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionAnnualReportDTO.RegionId && a.Date.Year == regionAnnualReportDTO.Date.Year);
            if (await CheckCreatedAsync(region.ID, region.Date.Year))
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

        private async Task<bool> CheckCreatedAsync(int regionId, int year)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionId && a.Date.Year == year) != null;
        }

        public async Task<RegionAnnualReportDTO> GetReportByIdAsync(int id, int year)
        {
            return _mapper.Map<RegionAnnualReport, RegionAnnualReportDTO>(await _repositoryWrapper.RegionAnnualReports.GetFirstAsync(predicate: i => i.ID == id && i.Date.Year == year));
        }

        public async Task<IEnumerable<RegionAnnualReportDTO>> GetAllRegionsReportsAsync()
        {
            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDTO>>(await _repositoryWrapper.RegionAnnualReports.FindAll().ToListAsync());
        }

        public async Task<IEnumerable<RegionAnnualReportTableObject>> GetAllRegionsReportsAsync(string searchedData, int page, int pageSize, int sortKey)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetRegionAnnualReportsAsync(searchedData, page, pageSize, sortKey);
        }

    }
}
