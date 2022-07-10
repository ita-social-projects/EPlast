using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ClubMemberHistoryDto>> GetClubReportMembersAsync(int ClubAnnualReportID)
        {
            var clubReportMember = await _repositoryWrapper.ClubReportMember.GetAllAsync(predicate: a => a.ClubAnnualReportId == ClubAnnualReportID,
                                                                      include: source => source
                                                                               .Include(a => a.ClubMemberHistory.User.ClubReportPlastDegrees.PlastDegree)
                                                                               .Include(x => x.ClubMemberHistory.User.ClubReportCities.City));

            var users = _mapper.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDto>>(clubReportMember.Select(user => user.ClubMemberHistory));
            return users;
        }

        public async Task<IEnumerable<ClubReportAdministrationDto>> GetClubReportAdminsAsync(int ClubAnnualReportID)
        {
            var clubReportAdmins = await _repositoryWrapper.ClubReportAdmins.GetAllAsync(predicate: a => a.ClubAnnualReportId == ClubAnnualReportID,
             include: source => source
                       .Include(a => a.ClubAdministration).ThenInclude(t => t.AdminType)
                       .Include(a => a.ClubAdministration.User.ClubReportPlastDegrees.PlastDegree)
                       .Include(a => a.ClubAdministration.User.ClubReportCities.City));

            var admins = _mapper.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubReportAdministrationDto>>(clubReportAdmins.Select(admin => admin.ClubAdministration));
            return admins;
        }



        ///<inheritdoc/>
        public async Task<ClubAnnualReportDto> GetByIdAsync(User user, int id)
        {
            var clubReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(predicate: a => a.ID == id);

            if (!await _clubAccessService.HasAccessAsync(user, clubReport.ClubId) ||
               (!(await _userManager.GetRolesAsync(user)).Any(x => Roles.HeadsAndHeadDeputiesAndAdmin.Contains(x))))
            {
                throw new UnauthorizedAccessException();
            }

            var users = await GetClubReportMembersAsync(id);
            var admins = await GetClubReportAdminsAsync(id);

            var clubReportDTO = _mapper.Map<ClubAnnualReport, ClubAnnualReportDto>(clubReport);

            clubReportDTO.Head = admins.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead);
            clubReportDTO.Admins = admins.ToList();

            clubReportDTO.Members = (from members in users
                                     where (!members.IsFollower)
                                     select members).ToList();

            clubReportDTO.Followers = (from folowers in users
                                       where (folowers.IsFollower)
                                       select folowers).ToList();
            return clubReportDTO;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<ClubAnnualReportDto>> GetAllAsync(User user)
        {
            var annualReports = await _repositoryWrapper.ClubAnnualReports.GetAllAsync(
                    include: source => source
                        .Include(ar => ar.Club)
                            .ThenInclude(c => c.ClubAdministration)
                        .Include(ca => ca.Club)
                            .ThenInclude(cm => cm.ClubMembers));
            return _mapper.Map<IEnumerable<ClubAnnualReport>, IEnumerable<ClubAnnualReportDto>>(annualReports);
        }

        public async Task<IEnumerable<ClubAnnualReportTableObject>> GetAllAsync(User user, bool isAdmin, string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            return await _repositoryWrapper.ClubAnnualReports.GetClubAnnualReportsAsync(user.Id, isAdmin, searchedData, page, pageSize, sortKey, auth);
        }

        private void SetClubReportMembersOrFollowers(IEnumerable<ClubMemberHistoryDto> ReportMembersOrFollowers, List<ClubReportMember> reportUsers, List<ClubReportPlastDegrees> reportUsersPlastDegrees, List<ClubReportCities> reportUsersCity, int clubAnnualReportID)
        {
            foreach (var clubHistoryMember in ReportMembersOrFollowers)
            {
                reportUsers.Add(new ClubReportMember { ClubAnnualReportId = clubAnnualReportID, ClubMemberHistoryId = clubHistoryMember.ID });

                if (clubHistoryMember.User.UserPlastDegrees != null)
                {
                    reportUsersPlastDegrees.Add(new ClubReportPlastDegrees
                    {
                        ClubAnnualReportId = clubAnnualReportID,
                        UserId = clubHistoryMember.User.ID,
                        PlastDegreeId = clubHistoryMember.User.UserPlastDegrees.PlastDegree.Id
                    });
                }
                if (clubHistoryMember.User.CityMembers.Count > 0)
                {
                    reportUsersCity.Add(new ClubReportCities
                    {
                        ClubAnnualReportId = clubAnnualReportID,
                        UserId = clubHistoryMember.User.ID,
                        CityId = clubHistoryMember.User.CityMembers.AsEnumerable().First().CityId
                    });
                }
            }
        }

        private void SetReportAdmins(IEnumerable<ClubReportAdministrationDto> Admins, List<ClubReportAdmins> reportAdmins, int clubAnnualReportID)
        {
            foreach (var clubReportAdmin in Admins)
            {
                reportAdmins.Add(new ClubReportAdmins { ClubAnnualReportId = clubAnnualReportID, ClubAdministrationId = clubReportAdmin.ID });
            }
        }


        public async Task CreateAsync(User user, ClubAnnualReportDto clubAnnualReportDTO)
        {
            var club = await _repositoryWrapper.Club.GetFirstOrDefaultAsync(
                predicate: a => a.ID == clubAnnualReportDTO.ClubId);

            if (await CheckCreated(club.ID))
            {
                throw new InvalidOperationException();
            }

            if (!await _clubAccessService.HasAccessAsync(user, club.ID))
            {
                throw new UnauthorizedAccessException();
            }

            clubAnnualReportDTO.ClubName = club.Name;
            var clubAnnualReport = _mapper.Map<ClubAnnualReportDto, ClubAnnualReport>(clubAnnualReportDTO);

            await _repositoryWrapper.ClubAnnualReports.CreateAsync(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();


            List<ClubReportMember> reportUsers = new List<ClubReportMember>();
            List<ClubReportCities> reportUsersCity = new List<ClubReportCities>();
            List<ClubReportPlastDegrees> reportUsersReportPlastDegrees = new List<ClubReportPlastDegrees>();
            List<ClubReportAdmins> reportAdmins = new List<ClubReportAdmins>();

            SetClubReportMembersOrFollowers(clubAnnualReportDTO.Members,
                                            reportUsers,
                                            reportUsersReportPlastDegrees,
                                            reportUsersCity,
                                            clubAnnualReport.ID
                                            );

            SetClubReportMembersOrFollowers(clubAnnualReportDTO.Followers,
                                   reportUsers,
                                   reportUsersReportPlastDegrees,
                                   reportUsersCity,
                                   clubAnnualReport.ID
                                   );

            SetReportAdmins(clubAnnualReportDTO.Admins, reportAdmins, clubAnnualReport.ID);

            reportAdmins.ForEach(n => _repositoryWrapper.ClubReportAdmins.CreateAsync(n));
            reportUsers.ForEach(n => _repositoryWrapper.ClubReportMember.CreateAsync(n));
            reportUsersCity.ForEach(n => _repositoryWrapper.ClubReportCities.CreateAsync(n));
            reportUsersReportPlastDegrees.ForEach(n => _repositoryWrapper.ClubReportPlastDegrees.CreateAsync(n));

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
        public async Task RemoveClubReportAsync(int id)
        {
            var clubAnnualReport = await _repositoryWrapper.ClubAnnualReports.GetFirstOrDefaultAsync(a => a.ClubId == id);
            _repositoryWrapper.ClubAnnualReports.Delete(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }

        ///<inheritdoc/>
        public async Task EditClubReportAsync(User user, ClubAnnualReportDto clubAnnualReportDto)
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
            clubAnnualReport = _mapper.Map<ClubAnnualReportDto, ClubAnnualReport>(clubAnnualReportDto);
            _repositoryWrapper.ClubAnnualReports.Update(clubAnnualReport);
            await _repositoryWrapper.SaveAsync();
        }
    }

}
