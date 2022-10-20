using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

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
        public async Task<RegionAnnualReportDto> CreateByNameAsync(User claimsPrincipal, int id, int year,
            RegionAnnualReportQuestions regionAnnualReportQuestions)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(a => a.ID == id);
            var user = await _repositoryWrapper.User.GetFirstOrDefaultAsync(u => u.Id == claimsPrincipal.Id);

            if (!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException("User doesn't have access");
            }

            if (await CheckCreatedAsync(id, year))
            {
                throw new InvalidOperationException("Report is already created!");
            }

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

                CreatorId = user.Id,

                CreatorFirstName = user.FirstName,

                CreatorLastName = user.LastName,

                CreatorFatherName = user.FatherName,

                Date = reportDate,

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
            regionAnnualReport = await SetMembersInfoAsync(regionAnnualReport);

            _repositoryWrapper.RegionAnnualReports.Create(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<RegionAnnualReportDto>(regionAnnualReport);
        }

        public async Task UpdateMembersInfo(int regionId, int year)
        {
            var regionReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionId && a.Date.Year == year);
            if (regionReport == null)
                return;
            regionReport = await SetMembersInfoAsync(regionReport);
            _repositoryWrapper.RegionAnnualReports.Update(regionReport);
            await _repositoryWrapper.SaveAsync();
        }

        private int CheckNull(int? number)
        {
            return number == null ? 0 : (int) number;
        }

        private async Task<RegionAnnualReport> SetMembersInfoAsync(RegionAnnualReport regionAnnualReport)
        {
            var membersIfo =
                (await _repositoryWrapper.RegionAnnualReports.GetRegionMembersInfoAsync(
                    regionId: regionAnnualReport.RegionId,
                    year: regionAnnualReport.Date.Year, getGeneral: true, page: 1, pageSize: 1)).ToList()[0];
            regionAnnualReport.NumberOfSeigneurMembers = CheckNull(membersIfo.NumberOfSeigneurMembers);

            regionAnnualReport.NumberOfSeigneurSupporters = CheckNull(membersIfo.NumberOfSeigneurSupporters);

            regionAnnualReport.NumberOfSeniorPlastynMembers = CheckNull(membersIfo.NumberOfSeniorPlastynMembers);

            regionAnnualReport.NumberOfSeniorPlastynSupporters = CheckNull(membersIfo.NumberOfSeniorPlastynSupporters);

            regionAnnualReport.NumberOfUnatstvaSkobVirlyts = CheckNull(membersIfo.NumberOfUnatstvaSkobVirlyts);

            regionAnnualReport.NumberOfUnatstvaProspectors = CheckNull(membersIfo.NumberOfUnatstvaProspectors);

            regionAnnualReport.NumberOfUnatstvaMembers = CheckNull(membersIfo.NumberOfUnatstvaMembers);

            regionAnnualReport.NumberOfUnatstvaSupporters = CheckNull(membersIfo.NumberOfUnatstvaSupporters);

            regionAnnualReport.NumberOfUnatstvaNoname = CheckNull(membersIfo.NumberOfUnatstvaNoname);

            regionAnnualReport.NumberOfNovatstva = CheckNull(membersIfo.NumberOfNovatstva);

            regionAnnualReport.NumberOfPtashata = CheckNull(membersIfo.NumberOfPtashata);

            regionAnnualReport.NumberOfSeatsPtashat = CheckNull(membersIfo.NumberOfSeatsPtashat);

            regionAnnualReport.NumberOfIndependentRiy = CheckNull(membersIfo.NumberOfIndependentRiy);

            regionAnnualReport.NumberOfClubs = CheckNull(membersIfo.NumberOfClubs);

            regionAnnualReport.NumberOfIndependentGroups = CheckNull(membersIfo.NumberOfIndependentGroups);

            regionAnnualReport.NumberOfPlastpryiatMembers = CheckNull(membersIfo.NumberOfPlastpryiatMembers);

            regionAnnualReport.NumberOfBeneficiaries = CheckNull(membersIfo.NumberOfBeneficiaries);

            regionAnnualReport.NumberOfHonoraryMembers = CheckNull(membersIfo.NumberOfHonoraryMembers);

            regionAnnualReport.NumberOfAdministrators = CheckNull(membersIfo.NumberOfAdministrators);

            regionAnnualReport.NumberOfTeacherAdministrators = CheckNull(membersIfo.NumberOfTeacherAdministrators);

            regionAnnualReport.NumberOfTeachers = CheckNull(membersIfo.NumberOfTeachers);

            return regionAnnualReport;
        }

        public async Task<IEnumerable<RegionMembersInfoTableObject>> GetRegionMembersInfoAsync(int regionId, int year, int page, int pageSize)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetRegionMembersInfoAsync(regionId, year,false, page, pageSize);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<RegionAnnualReportDto>> GetAllAsync(User claimsPrincipal)
        {
            var annualReports = await _repositoryWrapper.RegionAnnualReports.GetAllAsync(include:
                 source => source
                 .Include(a => a.Region));

            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDto>>(annualReports);
        }

        public async Task CreateAsync(User claimsPrincipal, RegionAnnualReportDto regionAnnualReportDTO)
        {
            var region = await _repositoryWrapper.Region.GetFirstOrDefaultAsync(
                predicate: a => a.ID == regionAnnualReportDTO.RegionId);
            if (await CheckCreatedAsync(region.ID, regionAnnualReportDTO.Date.Year))
            {
                throw new InvalidOperationException("Report is already crated!");
            }
            if (!await _regionAccessService.HasAccessAsync(claimsPrincipal, region.ID))
            {
                throw new UnauthorizedAccessException("No access for creating Annual Report!");
            }
            var regionAnnualReport = _mapper.Map<RegionAnnualReportDto, RegionAnnualReport>(regionAnnualReportDTO);
            regionAnnualReport.Date = DateTime.Now;
            await _repositoryWrapper.RegionAnnualReports.CreateAsync(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        private async Task<bool> CheckCreatedAsync(int regionId, int year)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionId && a.Date.Year == year) != null;
        }

        public async Task<RegionAnnualReportDto> GetReportByIdAsync(User claimsPrincipal, int id, int year)
        {
            var regionReport = await _repositoryWrapper.RegionAnnualReports.GetFirstAsync(predicate: i => i.ID == id && i.Date.Year == year);
            return await _regionAccessService.HasAccessAsync(claimsPrincipal, regionReport.RegionId) ? _mapper.Map<RegionAnnualReport, RegionAnnualReportDto>(regionReport)
                : throw new UnauthorizedAccessException();
        }

        public async Task<IEnumerable<RegionAnnualReportDto>> GetAllRegionsReportsAsync()
        {
            return _mapper.Map<IEnumerable<RegionAnnualReport>, IEnumerable<RegionAnnualReportDto>>(await Task.Run(() => _repositoryWrapper.RegionAnnualReports.FindAll().ToList()));
        }

        public async Task<IEnumerable<RegionAnnualReportTableObject>> GetAllRegionsReportsAsync(User user, bool isAdmin, string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            return await _repositoryWrapper.RegionAnnualReports.GetRegionAnnualReportsAsync(user.Id, isAdmin, searchedData, page, pageSize, sortKey, auth);
        }

        private async Task SaveLastConfirmedAsync(int regionId)
        {
            var regionAnnualReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.RegionId == regionId && a.Status == AnnualReportStatus.Confirmed);
            if (regionAnnualReport != null)
            {
                regionAnnualReport.Status = AnnualReportStatus.Saved;
                _repositoryWrapper.RegionAnnualReports.Update(regionAnnualReport);
            }
        }

        ///<inheritdoc/>
        public async Task ConfirmAsync(int id)
        {
            var regionAnnualReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed);
            regionAnnualReport.Status = AnnualReportStatus.Confirmed;
            _repositoryWrapper.RegionAnnualReports.Update(regionAnnualReport);
            await SaveLastConfirmedAsync(regionAnnualReport.RegionId);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task CancelAsync(int id)
        {
            var regionAnnualReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ID == id && a.Status == AnnualReportStatus.Confirmed);
            regionAnnualReport.Status = AnnualReportStatus.Unconfirmed;
            _repositoryWrapper.RegionAnnualReports.Update(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var regionAnnualReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed);
            _repositoryWrapper.RegionAnnualReports.Delete(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task EditAsync(int reportId, RegionAnnualReportQuestions regionAnnualReportQuestions)
        {
            var regionAnnualReport = await _repositoryWrapper.RegionAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ID == reportId && a.Status == AnnualReportStatus.Unconfirmed);
            if (regionAnnualReport.Status != AnnualReportStatus.Unconfirmed)
            {
                throw new InvalidOperationException();
            }

            regionAnnualReport.StateOfPreparation = regionAnnualReportQuestions.StateOfPreparation;

            regionAnnualReport.Characteristic = regionAnnualReportQuestions.Characteristic;

            regionAnnualReport.ChurchCooperation = regionAnnualReportQuestions.ChurchCooperation;

            regionAnnualReport.InvolvementOfVolunteers = regionAnnualReportQuestions.InvolvementOfVolunteers;

            regionAnnualReport.ImportantNeeds = regionAnnualReportQuestions.ImportantNeeds;

            regionAnnualReport.SocialProjects = regionAnnualReportQuestions.SocialProjects;

            regionAnnualReport.StatusOfStrategy = regionAnnualReportQuestions.StatusOfStrategy;

            regionAnnualReport.SuccessStories = regionAnnualReportQuestions.SuccessStories;

            regionAnnualReport.ProblemSituations = regionAnnualReportQuestions.ProblemSituations;

            regionAnnualReport.TrainedNeeds = regionAnnualReportQuestions.TrainedNeeds;

            regionAnnualReport.PublicFunding = regionAnnualReportQuestions.PublicFunding;

            regionAnnualReport.Fundraising = regionAnnualReportQuestions.Fundraising;

            regionAnnualReport = await SetMembersInfoAsync(regionAnnualReport);

            _repositoryWrapper.RegionAnnualReports.Update(regionAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<IEnumerable<RegionForAdministrationDto>> GetAllRegionsIdAndName(User user)
        {
            return (await _regionAccessService.GetAllRegionsIdAndName(user));
        }

    }
}
