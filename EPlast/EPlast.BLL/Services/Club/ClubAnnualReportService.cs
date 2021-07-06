using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.Club
{
    public class ClubAnnualReportService : IClubAnnualReportService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IClubAccessService _clubAccessService;
        private readonly IMapper _mapper;

        public ClubAnnualReportService(UserManager<User> userManager, IRepositoryWrapper repositoryWrapper,
                                    IClubAccessService clubAccessService, IMapper mapper)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _clubAccessService = clubAccessService;
            _mapper = mapper;
        }

        ///<inheritdoc/>

        public async Task<ClubAnnualReportDTO> GetByIdAsync(User user, int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id,
                    include: source => source
                        .Include(a => a.Club)
                            .ThenInclude(c => c.ClubAdministration)
                                .ThenInclude(cb => cb.AdminType)
                        .Include(ad => ad.Club)
                            .ThenInclude(ac => ac.ClubAdministration)
                                .ThenInclude(ar => ar.User)
                        .Include(ca => ca.Club)
                            .ThenInclude(cm => cm.ClubMembers)
                                .ThenInclude(mc => mc.User));
            if (await _clubAccessService.HasAccessAsync(user, clubAnnualReport.ClubId) ||
                (await _userManager.GetRolesAsync(user)).Any(x => Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy.Contains(x)))
                return _mapper.Map<ClubAnnualReport, ClubAnnualReportDTO>(clubAnnualReport);
            throw new UnauthorizedAccessException();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<ClubAnnualReportDTO>> GetAllAsync(User user)
        {
            var annualReports = await _repositoryWrapper.ClubAnnualReports.GetAllAsync(
                    include: source => source
                        .Include(ar => ar.Club)
                            .ThenInclude(c => c.ClubAdministration)
                        .Include(ca => ca.Club)
                            .ThenInclude(cm => cm.ClubMembers));
            return _mapper.Map<IEnumerable<ClubAnnualReport>, IEnumerable<ClubAnnualReportDTO>>(annualReports);
        }

        public async Task<IEnumerable<ClubAnnualReportTableObject>> GetAllAsync(User user, bool isAdmin, string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            return await _repositoryWrapper.ClubAnnualReports.GetClubAnnualReportsAsync(user.Id, isAdmin, searchedData, page, pageSize, sortKey, auth);
        }

        public async Task CreateAsync(User user, ClubAnnualReportDTO clubAnnualReportDTO)
        {
            var club = await _repositoryWrapper.Club.GetFirstOrDefaultAsync(
                predicate: a => a.ID == clubAnnualReportDTO.ClubId,
                include: source => source
                 .Include(a => a.ClubMembers)
                     .ThenInclude(ac => ac.User)
                         .ThenInclude(af => af.UserPlastDegrees)
                             .ThenInclude(ad => ad.PlastDegree)
                 .Include(a => a.ClubAdministration)
                     .ThenInclude(ap => ap.User)
                         .ThenInclude(af => af.UserPlastDegrees)
                             .ThenInclude(ad => ad.PlastDegree)

                );

            if (await CheckCreated(club.ID))
            {
                throw new InvalidOperationException();
            }
            if (!await _clubAccessService.HasAccessAsync(user, club.ID))
            {
                throw new UnauthorizedAccessException();
            }

            StringBuilder clubMembers = new StringBuilder();
            foreach (var item in club.ClubMembers)
            {
                var userPlastDegrees = await _repositoryWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == item.UserId, include: pd => pd.Include(d => d.PlastDegree));
                var degree = userPlastDegrees.FirstOrDefault(u=> u.UserId == item.UserId);
                var cityMember = await _repositoryWrapper.CityMembers.GetFirstOrDefaultAsync(predicate: a => a.UserId == item.UserId, include: source => source.Include(ar => ar.City));

                clubMembers.Append(new StringBuilder($"{degree?.PlastDegree.Name.TrimEnd()}, {item.User.FirstName} {item.User.LastName}, {cityMember?.City.Name}\n"));
            }

            clubAnnualReportDTO.ClubMembersSummary = clubMembers.ToString();

            StringBuilder clubAdmins = new StringBuilder();
            foreach (var item in club.ClubAdministration)
            {
                var userPlastDegrees = await _repositoryWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == item.UserId, include: pd => pd.Include(d => d.PlastDegree));
                var degree = userPlastDegrees.FirstOrDefault(user => user.UserId == item.UserId);
                if (item.AdminTypeId == 69)
                {
                    clubAdmins.Append(new StringBuilder(
                        $"{degree?.PlastDegree.Name.TrimEnd()}, {item.User.FirstName} {item.User.LastName}, {item.User.Email}, {item.User.PhoneNumber}\n"));
                }
            }

            clubAnnualReportDTO.ClubAdminContacts = clubAdmins.ToString();
            clubAnnualReportDTO.ClubName = club.Name;
            var clubAnnualReport = _mapper.Map<ClubAnnualReportDTO, ClubAnnualReport>(clubAnnualReportDTO);

            await _repositoryWrapper.ClubAnnualReports.CreateAsync(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();

        }

        private async Task<bool> CheckCreated(int Id)
        {
            return await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.Club.ID == Id && a.Date.Year == DateTime.Now.Year) != null;
        }

        ///<inheritdoc/>
        public async Task<bool> CheckCreated(User user, int clubId)
        {
            if (!await _clubAccessService.HasAccessAsync(user, clubId))
            {
                throw new UnauthorizedAccessException();
            }
            return await CheckCreated(clubId);
        }

        ///<inheritdoc/>
        public async Task ConfirmAsync(User user, int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed);
            if (!await _clubAccessService.HasAccessAsync(user, clubAnnualReport.ClubId))
            {
                throw new UnauthorizedAccessException();
            }

            clubAnnualReport.Status = AnnualReportStatus.Confirmed;
            _repositoryWrapper.ClubAnnualReports.Update(clubAnnualReport);
            await SaveLastConfirmedAsync(clubAnnualReport.ClubId);
            await _repositoryWrapper.SaveAsync();
        }

        private async Task SaveLastConfirmedAsync(int clubId)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                predicate: a => a.ClubId == clubId && a.Status == AnnualReportStatus.Confirmed);
            if (clubAnnualReport != null)
            {
                clubAnnualReport.Status = AnnualReportStatus.Saved;
                _repositoryWrapper.ClubAnnualReports.Update(clubAnnualReport);
            }
        }

        ///<inheritdoc/>
        public async Task CancelAsync(User user, int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Confirmed);
            if (!await _clubAccessService.HasAccessAsync(user, clubAnnualReport.ClubId))
            {
                throw new UnauthorizedAccessException();
            }
            clubAnnualReport.Status = AnnualReportStatus.Unconfirmed;
            _repositoryWrapper.ClubAnnualReports.Update(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task DeleteClubReportAsync(User user, int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ID == id && a.Status == AnnualReportStatus.Unconfirmed);
            if (!await _clubAccessService.HasAccessAsync(user, clubAnnualReport.ClubId))
            {
                throw new UnauthorizedAccessException();
            }
            _repositoryWrapper.ClubAnnualReports.Delete(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task EditClubReportAsync(User user, ClubAnnualReportDTO clubAnnualReportDto)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(
                    predicate: a => a.ClubId == clubAnnualReportDto.ClubId
                      && a.Status == AnnualReportStatus.Unconfirmed);
            if (clubAnnualReportDto.Status != AnnualReportStatus.Unconfirmed)
            {
                throw new InvalidOperationException();
            }
            if (!await _clubAccessService.HasAccessAsync(user, clubAnnualReport.ClubId))
            {
                throw new UnauthorizedAccessException();
            }
            clubAnnualReport = _mapper.Map<ClubAnnualReportDTO, ClubAnnualReport>(clubAnnualReportDto);
            _repositoryWrapper.ClubAnnualReports.Update(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }
    }

}
