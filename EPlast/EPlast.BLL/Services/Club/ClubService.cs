using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Services.CityClub;
using DataAccessClub = EPlast.DataAccess.Entities;
using EPlast.Resources;
using System.Linq.Expressions;

namespace EPlast.BLL.Services.Club
{
    public class ClubService : CityClubBase, IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IClubBlobStorageRepository _clubBlobStorage;
        private readonly UserManager<DataAccessClub.User> _userManager;
        private readonly IUniqueIdService _uniqueId;

        private const int MembersDisplayCount = 9;
        private const int FollowersDisplayCount = 6;
        private const int DocumentsDisplayCount = 6;
        private const int AdminsDisplayCount = 6;

        public ClubService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IWebHostEnvironment env,
            IClubBlobStorageRepository clubBlobStorage,
            IClubAccessService clubAccessService,
            UserManager<DataAccessClub.User> userManager,
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _clubBlobStorage = clubBlobStorage;
            _userManager = userManager;
            _uniqueId = uniqueId;
        }

        public async Task ArchiveAsync(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == clubId && c.IsActive);
            if (club.ClubMembers is null && club.ClubAdministration is null)
            {
                club.IsActive = false;
                _repoWrapper.Club.Update(club);
                await _repoWrapper.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DataAccessClub.Club>> GetAllAsync(string clubName = null)
        {
            var cities = await _repoWrapper.Club.GetAllAsync();

            return string.IsNullOrEmpty(clubName)
                ? cities
                : cities.Where(c => c.Name.ToLower().Contains(clubName.ToLower()));
        }

        public async Task<IEnumerable<DataAccessClub.Club>> GetAllActiveAsync(string clubName = null)
        {
            var clubs = await _repoWrapper.Club.GetAllAsync();
            var filteredClubs = clubs.Where(c => c.IsActive);
            return string.IsNullOrEmpty(clubName)
                ? filteredClubs
                : filteredClubs.Where(c => c.Name.ToLower().Contains(clubName.ToLower()));
        }

        public async Task<IEnumerable<DataAccessClub.Club>> GetAllNotActiveAsync(string clubName = null)
        {
            var clubs = await _repoWrapper.Club.GetAllAsync();
            var filteredClubs = clubs.Where(c => !c.IsActive);
            return string.IsNullOrEmpty(clubName)
                ? filteredClubs
                : filteredClubs.Where(c => c.Name.ToLower().Contains(clubName.ToLower()));
        }

        public async Task<IEnumerable<ClubDTO>> GetAllActiveClubsAsync(string clubName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(await GetAllActiveAsync(clubName));
        }

        public async Task<IEnumerable<ClubDTO>> GetAllNotActiveClubsAsync(string clubName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(await GetAllNotActiveAsync(clubName));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubDTO>> GetAllClubsAsync(string clubName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(await GetAllAsync(clubName));
        }

        /// <inheritdoc />
        public async Task<ClubDTO> GetByIdAsync(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == clubId,
                    include: source => source
                       .Include(c => c.ClubAdministration)
                           .ThenInclude(t => t.AdminType)
                       .Include(k => k.ClubAdministration)
                           .ThenInclude(a => a.User)
                       .Include(m => m.ClubMembers)
                           .ThenInclude(u => u.User)
                       .Include(l => l.ClubDocuments)
                           .ThenInclude(d => d.ClubDocumentType));

            return _mapper.Map<DataAccessClub.Club, ClubDTO>(club);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubUserDTO>> GetClubUsersAsync(int clubId)
        {
            var clubMembers = await _repoWrapper.ClubMembers.GetAllAsync(d => d.ClubId == clubId && d.IsApproved,
                include: source => source
                    .Include(t => t.User));
            var users = clubMembers.Select(x => x.User);
            return _mapper.Map<IEnumerable<DataAccessClub.User>, IEnumerable<ClubUserDTO>>(users);
        }

        private async Task<ClubProfileDTO> GetClubInfoAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }
            var clubHead = club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead && a.Status);
            var clubHeadDeputy = club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHeadDeputy && a.Status);
            var clubAdmins = club.ClubAdministration?
                .Where(a => a.Status)
                .ToList();
            club.AdministrationCount = club.ClubAdministration == null ? 0
                : club.ClubAdministration.Count(a => a.Status);
            var members = club.ClubMembers
                .Where(m => m.IsApproved)
                .ToList();
            club.MemberCount = club.ClubMembers
                .Count(m => m.IsApproved);
            var followers = club.ClubMembers
                .Where(m => !m.IsApproved)
                .ToList();
            club.FollowerCount = club.ClubMembers
                .Count(m => !m.IsApproved);
            club.DocumentsCount = club.ClubDocuments.Count();
            var clubDoc = club.ClubDocuments.Take(6).ToList();
            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Head = clubHead,
                HeadDeputy = clubHeadDeputy,
                Members = members,
                Followers = followers,
                Admins = clubAdmins,
                Documents = clubDoc,
            };
            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubMembersInfoAsync(int clubId)
        {
            var club = await GetClubInfoAsync(clubId);
            if (club == null)
            {
                return null;
            }
            club.Head = (await setMembersCityName(new List<ClubAdministrationDTO>() { club.Head! })).FirstOrDefault() as ClubAdministrationDTO;
            club.HeadDeputy =
                (await setMembersCityName(new List<ClubAdministrationDTO>() { club.HeadDeputy! })).FirstOrDefault() as
                ClubAdministrationDTO;
            club.Members = await setMembersCityName(club.Members) as List<ClubMembersDTO>;
            club.Followers = await setMembersCityName(club.Followers) as List<ClubMembersDTO>;
            if (club.Admins != null)
                club.Admins = await setMembersCityName(club.Admins) as List<ClubAdministrationDTO>;
            club.Documents = null;

            return club;
        }

        public async Task<ClubProfileDTO> GetClubProfileAsync(int clubId)
        {
            var club = await GetClubInfoAsync(clubId);
            if (club == null)
            {
                return null;
            }

            club.Members = club.Members.Take(MembersDisplayCount).ToList();
            club.Followers = club.Followers.Take(FollowersDisplayCount).ToList();
            club.Documents = club.Documents.Take(DocumentsDisplayCount).ToList();
            club.Admins = club.Admins.Take(AdminsDisplayCount).ToList();

            return club;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubProfileAsync(int clubId, DataAccessClub.User user)
        {
            var clubProfileDto = await GetClubProfileAsync(clubId);
            var userId = await _userManager.GetUserIdAsync(user);

            clubProfileDto.Club.CanJoin = (await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.User.Id == userId && u.ClubId == clubId)) == null;

            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubMembersAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }

            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Members = await setMembersCityName(club.ClubMembers
                    .Where(m => m.IsApproved)
                    .ToList()) as List<ClubMembersDTO>
            };

            return clubProfileDto;
        }

        private async Task<IEnumerable<IClubMember>> setMembersCityName(IEnumerable<IClubMember> members)
        {
            foreach (var member in members.Where(m => m != null))
            {
                var userId = member.UserId;
                var cityMembers = await _repoWrapper.CityMembers.GetFirstOrDefaultAsync(a => a.UserId == userId);
                if (cityMembers != null)
                {
                    var city = await _repoWrapper.City.GetFirstAsync(a => a.ID == cityMembers.CityId);
                    member.User.CityName = city.Name.ToString();
                }
            }
            return members;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubFollowersAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }

            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Followers = await setMembersCityName(club.ClubMembers
                    .Where(m => !m.IsApproved)
                    .ToList()) as List<ClubMembersDTO>
            };

            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubAdminsAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }

            var clubHead = await GetClubHeadAsync(clubId);
            var clubHeadDeputy = await GetClubHeadDeputyAsync(clubId);
            var clubAdmins = await GetAdminsAsync(clubId);

            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Admins = clubAdmins,
                Head = clubHead,
                HeadDeputy = clubHeadDeputy
            };

            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubDocumentsAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }

            var clubDoc = DocumentsSorter<ClubDocumentsDTO>.SortDocumentsBySubmitDate(club.ClubDocuments);

            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Documents = clubDoc.ToList()
            };

            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _clubBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == clubId);
            if (club.Logo != null)
            {
                await _clubBlobStorage.DeleteBlobAsync(club.Logo);
            }
            await DeleteClubMemberHistory(clubId);
            _repoWrapper.Club.Delete(club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> EditAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            if (club == null)
            {
                return null;
            }

            var clubAdmins = club.ClubAdministration
                .ToList();
            var members = club.ClubMembers
                .Where(p => clubAdmins.All(a => a.UserId != p.UserId))
                .Where(m => m.IsApproved)
                .ToList();
            var followers = club.ClubMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var clubProfileDto = new ClubProfileDTO
            {
                Club = club,
                Admins = clubAdmins,
                Members = members,
                Followers = followers
            };

            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task EditAsync(ClubProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.Club, file);
            var club = CreateClubFromProfileAsync(model);

            _repoWrapper.Club.Attach(club);
            _repoWrapper.Club.Update(club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task EditAsync(ClubDTO model)
        {
            await UploadPhotoAsync(model);
            var club = CreateClubAsync(model);

            _repoWrapper.Club.Attach(club);
            _repoWrapper.Club.Update(club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(ClubProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.Club, file);
            var club = CreateClubFromProfileAsync(model);
            _repoWrapper.Club.Attach(club);
            await _repoWrapper.Club.CreateAsync(club);
            await _repoWrapper.SaveAsync();

            return club.ID;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(ClubDTO model)
        {
            if (await CheckCreated(model.Name))
            {
                throw new InvalidOperationException();
            }

            await UploadPhotoAsync(model);
            var club = CreateClubAsync(model);

            _repoWrapper.Club.Attach(club);
            await _repoWrapper.Club.CreateAsync(club);
            await _repoWrapper.SaveAsync();

            return club.ID;
        }

        private async Task<bool> CheckCreated(string name)
        {
            return await _repoWrapper.Club.GetFirstOrDefaultAsync(
                predicate: a => a.Name == name) != null;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubForAdministrationDTO>> GetClubs()
        {
            var clubs = await _repoWrapper.Club.GetAllAsync();
            var filteredClubs = clubs.Where(c => c.IsActive);
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubForAdministrationDTO>>(filteredClubs);
        }

        private DataAccessClub.Club CreateClubFromProfileAsync(ClubProfileDTO model)
        {
            var clubDto = model.Club;

            var club = _mapper.Map<ClubDTO, DataAccessClub.Club>(clubDto);

            return club;
        }

        private DataAccessClub.Club CreateClubAsync(ClubDTO model)
        {
            var club = _mapper.Map<ClubDTO, DataAccessClub.Club>(model);

            return club;
        }

        private async Task UploadPhotoAsync(ClubDTO club, IFormFile file)
        {
            var clubId = club.ID;
            var oldImageName = (await _repoWrapper.Club.GetFirstOrDefaultAsync(
                predicate: i => i.ID == clubId))
                ?.Logo;

            club.Logo = GetChangedPhoto("images\\Clubs", file, oldImageName, _env.WebRootPath, _uniqueId.GetUniqueId().ToString());
        }

        private async Task UploadPhotoAsync(ClubDTO club)
        {
            var oldImageName = (await _repoWrapper.Club.GetFirstOrDefaultAsync(i => i.ID == club.ID))?.Logo;
            var logoBase64 = club.Logo;

            if (!string.IsNullOrWhiteSpace(logoBase64) && logoBase64.Length > 0)
            {
                var logoBase64Parts = logoBase64.Split(',');
                var extension = logoBase64Parts[0].Split(new[] { '/', ';' }, 3)[1];

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = (extension[0] == '.' ? "" : ".") + extension;
                }

                var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

                await _clubBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                club.Logo = fileName;
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                await _clubBlobStorage.DeleteBlobAsync(oldImageName);
            }
        }

        public async Task<IEnumerable<ClubMemberHistoryDTO>> GetClubHistoryFollowers(int clubId)
        {
            var clubHistoryFollowers = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                             predicate: c => c.ClubId == clubId &&
                                                        c.IsFollower &&
                                                        !c.IsDeleted,
                                              include: source => source
                                                     .Include(x => x.User).ThenInclude(x => x.UserPlastDegrees)
                                                                          .ThenInclude(x => x.PlastDegree)
                                                     .Include(d => d.User).ThenInclude(c => c.CityMembers)
                                                                          .ThenInclude(c => c.City));

            return _mapper.Map<IEnumerable<DataAccessClub.ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>(clubHistoryFollowers);
        }

        public async Task<IEnumerable<ClubMemberHistoryDTO>> GetClubHistoryMembers(int clubId)
        {
            
            var clubHistoryMembers = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                          predicate: c => c.ClubId == clubId &&
                                                     !c.IsFollower &&
                                                     !c.IsDeleted,
                                          include: source => source
                                                     .Include(x => x.User).ThenInclude(x => x.UserPlastDegrees)
                                                                          .ThenInclude(x => x.PlastDegree)
                                                     .Include(d => d.User).ThenInclude(c => c.CityMembers)
                                                                          .ThenInclude(c => c.City));

            return _mapper.Map<IEnumerable<DataAccessClub.ClubMemberHistory>, IEnumerable<ClubMemberHistoryDTO>>(clubHistoryMembers);
        }

        public async Task<IEnumerable<DataAccessClub.ClubAdministration>> GetClubAdministrations(int clubId)
        {
            var clubAdminins = await _repoWrapper.ClubAdministration.GetAllAsync(
                                     predicate: c => c.ClubId == clubId && c.Status,
                                     include: source => source
                                      .Include(t => t.AdminType)
                                      .Include(u => u.User).ThenInclude(u => u.CityMembers)
                                                           .ThenInclude(u => u.City)
                                      .Include(d => d.User).ThenInclude(d => d.UserPlastDegrees)
                                                           .ThenInclude(d => d.PlastDegree));
            return clubAdminins;
        }

        public async Task DeleteClubMemberHistory(int id)
        {
            var members = await _repoWrapper.ClubMemberHistory.GetAllAsync(c => c.ClubId == id);
            foreach (var VARIABLE in members)
            {
                _repoWrapper.ClubMemberHistory.Delete(VARIABLE);
            }
            await _repoWrapper.SaveAsync();
        }

        public async Task<int> GetCountUsersPerYear(int clubId)
        {
            var usersPerYear = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                       predicate: c => c.ClubId == clubId &&
                                                  !c.IsFollower && c.Date.Year == DateTime.Now.Year);
            return usersPerYear.Count();
        }
        public async Task<int> GetCountDeletedUsersPerYear(int clubId)
        {
            var deletedUsersPerYear = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                     predicate: c => c.ClubId == clubId && !c.IsFollower &&
                                                c.IsDeleted && c.Date.Year == DateTime.Now.Year);

            return deletedUsersPerYear.Count();
        }
        public async Task<ClubReportDataDTO> GetClubDataForReport(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(
                predicate: c => c.ID == clubId);

            if (club == null)
            {
                return null;
            }

            var clubAdmins = await GetClubAdministrations(clubId);
            var clubDto = _mapper.Map<DataAccessClub.Club, ClubDTO>(club);

            var clubHead = clubAdmins.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead);
            var head = _mapper.Map<DataAccessClub.ClubAdministration, ClubReportAdministrationDTO>(clubHead);
            var clubAdminsDto = _mapper.Map<IEnumerable<DataAccessClub.ClubAdministration>, IEnumerable<ClubReportAdministrationDTO>>(clubAdmins);

            var clubHistoryFollowersDTO = await GetClubHistoryFollowers(clubId);
            var clubHistoryMembersDTO = await GetClubHistoryMembers(clubId);


            var clubProfileDto = new ClubReportDataDTO
            {
                Club = clubDto,
                Head = head,
                Members = clubHistoryMembersDTO.ToList(),
                Followers = clubHistoryFollowersDTO.ToList(),
                Admins = clubAdminsDto.ToList(),
                CountUsersPerYear = await GetCountUsersPerYear(clubId),
                CountDeletedUsersPerYear = await GetCountDeletedUsersPerYear(clubId),
                PhoneNumber = club.PhoneNumber,
                Email = club.Email,
                ClubURL = club.ClubURL,
                Slogan = club.Slogan
            };
            return clubProfileDto;
        }

        public async Task UnArchiveAsync(int clubId)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == clubId && !c.IsActive);
            club.IsActive = true;
            _repoWrapper.Club.Update(club);
            await _repoWrapper.SaveAsync();
        }


        public async Task<Tuple<IEnumerable<ClubObjectDTO>, int>> GetAllClubsByPageAndIsArchiveAsync(int page, int pageSize, string clubName, bool isArchive)
        {
            var filter = GetFilter(clubName, isArchive);
            var order = GetOrder();
            var selector = GetSelector();
            var tuple = await _repoWrapper.Club.GetRange(filter, selector, order, page, pageSize);
            var clubs = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<ClubObjectDTO>, int>(_mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubObjectDTO>>(clubs), rows);
        }

        public async Task<ClubAdministrationDTO> GetClubHeadAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            return club.ClubAdministration?
                   .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead
                       && a.Status);
        }

        public async Task<ClubAdministrationDTO> GetClubHeadDeputyAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            return club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHeadDeputy
                    && a.Status);
        }

        public async Task<List<ClubAdministrationDTO>> GetAdminsAsync(int clubId)
        {
            var club = await GetByIdAsync(clubId);
            return club.ClubAdministration?
                .Where(a => a.Status)
                .ToList();
        }

        private Expression<Func<DataAccessClub.Club, bool>> GetFilter(string clubName, bool isArchive)
        {
            var clubNameEmty = string.IsNullOrEmpty(clubName);
            Expression<Func<DataAccessClub.Club, bool>> expr = (clubNameEmty) switch
            {
                true => x => x.IsActive == isArchive,
                false => x => x.Name.Contains(clubName) && x.IsActive == isArchive
            };
            return expr;
        }

        private Expression<Func<DataAccessClub.Club, object>> GetOrder()
        {

            Expression<Func<DataAccessClub.Club, object>> expr = x => x.ID;
            return expr;
        }

        private Expression<Func<DataAccessClub.Club, DataAccessClub.Club>> GetSelector()
        {

            Expression<Func<DataAccessClub.Club, DataAccessClub.Club>> expr = x => new DataAccessClub.Club { ID = x.ID, Logo = x.Logo, Name = x.Name };
            return expr;
        }
    }
}
