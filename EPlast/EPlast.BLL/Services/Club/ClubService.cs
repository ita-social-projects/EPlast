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
using  EPlast.Resources;
using NLog.LayoutRenderers.Wrappers;

namespace EPlast.BLL.Services.Club
{
    public class ClubService : CityClubBase, IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IClubBlobStorageRepository _clubBlobStorage;
        private readonly IClubAccessService _clubAccessService;
        private readonly UserManager<DataAccessClub.User> _userManager;
        private readonly IUniqueIdService _uniqueId;

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
            _clubAccessService = clubAccessService;
            _userManager = userManager;
            _uniqueId = uniqueId;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DataAccessClub.Club>> GetAllAsync(string ClubName = null)
        {
            var cities = await _repoWrapper.Club.GetAllAsync();

            return string.IsNullOrEmpty(ClubName)
                ? cities
                : cities.Where(c => c.Name.ToLower().Contains(ClubName.ToLower()));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubDTO>> GetAllDTOAsync(string ClubName = null)
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(await GetAllAsync(ClubName));
        }



        /// <inheritdoc />
        public async Task<ClubDTO> GetByIdAsync(int ClubId)
        {
            var Club = await _repoWrapper.Club.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == ClubId,
                    include: source => source
                       .Include(c => c.ClubAdministration)
                           .ThenInclude(t => t.AdminType)
                       .Include(k => k.ClubAdministration)
                           .ThenInclude(a => a.User)
                       .Include(m => m.ClubMembers)
                           .ThenInclude(u => u.User)
                       .Include(l => l.ClubDocuments)
                           .ThenInclude(d => d.ClubDocumentType));

            return _mapper.Map<DataAccessClub.Club, ClubDTO>(Club);
        }

        private async Task<ClubProfileDTO> GetClubInfoAsync(int ClubId)
        {
            var club = await GetByIdAsync(ClubId);
            if (club == null)
            {
                return null;
            }
            var clubHead = club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead
                                     && (DateTime.Now < a.EndDate || a.EndDate == null));
            var clubHeadDeputy = club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHeadDeputy
                                     && (DateTime.Now < a.EndDate || a.EndDate == null));
            var clubAdmins = club.ClubAdministration
                .Where(a => a.AdminType.AdminTypeName != Roles.KurinHead
                            && a.AdminType.AdminTypeName != Roles.KurinHeadDeputy
                            && (DateTime.Now < a.EndDate || a.EndDate == null))
                .ToList();
            club.AdministrationCount = club.ClubAdministration
                .Count(a => (DateTime.Now < a.EndDate || a.EndDate == null));
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
                Members = members,
                Followers = followers,
                Admins = clubAdmins,
                Documents = clubDoc,
            };
            return clubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubMembersInfoAsync(int ClubId)
        {
            var Club = await GetClubInfoAsync(ClubId);
            if (Club == null)
            {
                return null;
            }
            Club.Head = (await setMembersCityName(new List<ClubAdministrationDTO>() { Club.Head! })).FirstOrDefault() as ClubAdministrationDTO;
            Club.HeadDeputy =
                (await setMembersCityName(new List<ClubAdministrationDTO>() {Club.HeadDeputy!})).FirstOrDefault() as
                ClubAdministrationDTO;
            Club.Members = await setMembersCityName(Club.Members) as List<ClubMembersDTO>;
            Club.Followers = await setMembersCityName(Club.Followers) as List<ClubMembersDTO>;
            Club.Admins = await setMembersCityName(Club.Admins) as List<ClubAdministrationDTO>;
            Club.Documents = null;

            return Club;
        }

        public async Task<ClubProfileDTO> GetClubProfileAsync(int ClubId)
        {
            var club = await GetClubInfoAsync(ClubId);
            if (club == null)
            {
                return null;
            }
            club.Members=club.Members.Take(9).ToList();
            club.Followers = club.Followers.Take(6).ToList();
            club.Documents = club.Documents.Take(6).ToList();
            club.Admins = club.Admins.Take(6).ToList();
            return club;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubProfileAsync(int ClubId, DataAccessClub.User user)
        {
            var ClubProfileDto = await GetClubProfileAsync(ClubId);
            var userId = await _userManager.GetUserIdAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var members = ClubProfileDto.Members.Where(m => m.IsApproved).ToList();
            var admins = ClubProfileDto.Admins;
            var followers = ClubProfileDto.Followers.Where(m => !m.IsApproved).ToList();

            foreach (var member in members)
            {
                var id = member.UserId;
                var userPlastDegrees = await _repoWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == id, include: pd => pd.Include(d => d.PlastDegree));
                var userDegree = userPlastDegrees?.FirstOrDefault(u => u.UserId == id)?.PlastDegree;
                member.User.PlastDegree = userDegree==null? null : new DataAccessClub.PlastDegree
                    {
                        Id = userDegree.Id,
                        Name = userDegree.Name,
                    };
                var cityMembers = await _repoWrapper.CityMembers.GetFirstOrDefaultAsync(a => a.UserId == id);
                if (cityMembers != null)
                {
                    var city = await _repoWrapper.City.GetFirstAsync(a => a.ID == cityMembers.CityId);
                    member.User.CityName = city.Name.ToString();
                }
            }

            foreach (var admin in admins)
            {
                var userPlastDegrees = await _repoWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == admin.UserId, include: pd => pd.Include(d => d.PlastDegree));
                var userDegree = userPlastDegrees?.FirstOrDefault(u => u.UserId == admin.UserId)?.PlastDegree;
                admin.User.PlastDegree = userDegree == null ? null : new DataAccessClub.PlastDegree
                {
                    Id = userDegree.Id,
                    Name = userDegree.Name,
                };
            }
            foreach (var follower in followers)
            {
                var userPlastDegrees = await _repoWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == follower.UserId, include: pd => pd.Include(d => d.PlastDegree));
                var userDegree = userPlastDegrees?.FirstOrDefault(u => u.UserId == follower.UserId)?.PlastDegree;
                follower.User.PlastDegree = userDegree == null ? null : new DataAccessClub.PlastDegree
                {
                    Id = userDegree.Id,
                    Name = userDegree.Name,
                };
            }

            ClubProfileDto.Club.CanCreate = userRoles.Contains(Roles.Admin);
            ClubProfileDto.Club.CanEdit = await _clubAccessService.HasAccessAsync(user, ClubId);
            ClubProfileDto.Club.CanJoin = (await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.User.Id == userId && u.ClubId == ClubId)) == null;

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubMembersAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Members = await setMembersCityName(Club.ClubMembers
                    .Where(m => m.IsApproved)
                    .ToList()) as List<ClubMembersDTO>
            };

            return ClubProfileDto;
        }

        private async Task<IEnumerable<IClubMember>> setMembersCityName(IEnumerable<IClubMember> members)
        {
            foreach (var member in members.Where(m=>m!=null))
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
        public async Task<ClubProfileDTO> GetClubFollowersAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Followers = await setMembersCityName(Club.ClubMembers
                    .Where(m => !m.IsApproved)
                    .ToList()) as List<ClubMembersDTO>
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubAdminsAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubHead = Club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHead
                    && (DateTime.Now < a.EndDate || a.EndDate == null));
            var ClubHeadDeputy = Club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.KurinHeadDeputy
                    && (DateTime.Now < a.EndDate || a.EndDate == null));

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Admins = await setMembersCityName(Club.ClubAdministration
                        .Where(a => a.AdminType.AdminTypeName != Roles.KurinHead
                            && a.AdminType.AdminTypeName != Roles.KurinHeadDeputy
                            && (DateTime.Now < a.EndDate || a.EndDate == null)).ToList()) as
                    List<ClubAdministrationDTO>,
                Head = (await setMembersCityName(new List<ClubAdministrationDTO>() { ClubHead })).FirstOrDefault() as ClubAdministrationDTO,
                HeadDeputy = (await setMembersCityName(new List<ClubAdministrationDTO>() { ClubHeadDeputy })).FirstOrDefault() as ClubAdministrationDTO
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubDocumentsAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubDoc = Club.ClubDocuments.ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Documents = ClubDoc
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<string> GetLogoBase64(string logoName)
        {
            var logoBase64 = await _clubBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(int ClubId)
        {
            var Club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == ClubId);

            if (Club.Logo != null)
            {
                await _clubBlobStorage.DeleteBlobAsync(Club.Logo);
            }

            _repoWrapper.Club.Delete(Club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> EditAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubAdmins = Club.ClubAdministration
                .ToList();
            var members = Club.ClubMembers
                .Where(p => ClubAdmins.All(a => a.UserId != p.UserId))
                .Where(m => m.IsApproved)
                .ToList();
            var followers = Club.ClubMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Admins = ClubAdmins,
                Members = members,
                Followers = followers
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task EditAsync(ClubProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.Club, file);
            var Club = CreateClubFromProfileAsync(model);

            _repoWrapper.Club.Attach(Club);
            _repoWrapper.Club.Update(Club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task EditAsync(ClubDTO model)
        {
            await UploadPhotoAsync(model);
            var Club = CreateClubAsync(model);

            _repoWrapper.Club.Attach(Club);
            _repoWrapper.Club.Update(Club);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(ClubProfileDTO model, IFormFile file)
        {
            await UploadPhotoAsync(model.Club, file);
            var Club = CreateClubFromProfileAsync(model);
            _repoWrapper.Club.Attach(Club);
            await _repoWrapper.Club.CreateAsync(Club);
            await _repoWrapper.SaveAsync();

            return Club.ID;
        }

        /// <inheritdoc />
        public async Task<int> CreateAsync(ClubDTO model)
        {
            if (await CheckCreated(model.Name))
            {
                throw new InvalidOperationException();
            }

            await UploadPhotoAsync(model);
            var Club = CreateClubAsync(model);

            _repoWrapper.Club.Attach(Club);
            await _repoWrapper.Club.CreateAsync(Club);
            await _repoWrapper.SaveAsync();

            return Club.ID;
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
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubForAdministrationDTO>>(clubs);
        }

        private DataAccessClub.Club CreateClubFromProfileAsync(ClubProfileDTO model)
        {
            var ClubDto = model.Club;

            var Club = _mapper.Map<ClubDTO, DataAccessClub.Club>(ClubDto);

            return Club;
        }

        private DataAccessClub.Club CreateClubAsync(ClubDTO model)
        {
            var Club = _mapper.Map<ClubDTO, DataAccessClub.Club>(model);

            return Club;
        }

        private async Task UploadPhotoAsync(ClubDTO club, IFormFile file)
        {
            var ClubId = club.ID;
            var oldImageName = (await _repoWrapper.Club.GetFirstOrDefaultAsync(
                predicate: i => i.ID == ClubId))
                ?.Logo;

            club.Logo = GetChangedPhoto("images\\Clubs",file,oldImageName, _env.WebRootPath, _uniqueId.GetUniqueId().ToString());
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
    }
}
