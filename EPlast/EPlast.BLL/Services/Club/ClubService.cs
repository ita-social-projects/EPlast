﻿using AutoMapper;
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
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.Services.CityClub;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Club
{
    public class ClubService : CityClubBase, IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IClubBlobStorageRepository _ClubBlobStorage;
        private readonly IClubAccessService _ClubAccessService;
        private readonly UserManager<DataAccessClub.User> _userManager;
        private readonly IUniqueIdService _uniqueId;

        public ClubService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IWebHostEnvironment env,
            IClubBlobStorageRepository ClubBlobStorage,
            IClubAccessService ClubAccessService,
            UserManager<DataAccessClub.User> userManager,
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _ClubBlobStorage = ClubBlobStorage;
            _ClubAccessService = ClubAccessService;
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

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubProfileAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var ClubHead = Club.ClubAdministration?
                .FirstOrDefault(a => a.AdminType.AdminTypeName == "Голова Куреня"
                    && (DateTime.Now < a.EndDate || a.EndDate == null));
            var ClubAdmins = Club.ClubAdministration
                .Where(a => a.AdminType.AdminTypeName != "Голова Куреня"
                    && (DateTime.Now < a.EndDate || a.EndDate == null))
                .Take(6)
                .ToList();
            Club.AdministrationCount = Club.ClubAdministration
                .Where(a => (DateTime.Now < a.EndDate || a.EndDate == null))
                .Count();
            var members = Club.ClubMembers
                .Where(m => m.IsApproved)
                .Take(9)
                .ToList();
            Club.MemberCount = Club.ClubMembers
                .Where(m => m.IsApproved)
                .Count();
            var followers = Club.ClubMembers
                .Where(m => !m.IsApproved)
                .Take(6)
                .ToList();
            Club.FollowerCount = Club.ClubMembers
                .Where(m => !m.IsApproved)
                .Count();
            var ClubDoc = Club.ClubDocuments.Take(6).ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Head = ClubHead,
                Members = members,
                Followers = followers,
                Admins = ClubAdmins,
                Documents = ClubDoc,
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubProfileAsync(int ClubId, ClaimsPrincipal user)
        {
            var ClubProfileDto = await GetClubProfileAsync(ClubId);
            var userId = _userManager.GetUserId(user);

            ClubProfileDto.Club.CanCreate = user.IsInRole("Admin");
            ClubProfileDto.Club.CanEdit = await _ClubAccessService.HasAccessAsync(user, ClubId);
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

            var members = Club.ClubMembers
                .Where(m => m.IsApproved)
                .ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Members = members
            };

            return ClubProfileDto;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubFollowersAsync(int ClubId)
        {
            var Club = await GetByIdAsync(ClubId);
            if (Club == null)
            {
                return null;
            }

            var followers = Club.ClubMembers
                .Where(m => !m.IsApproved)
                .ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Followers = followers
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
                .FirstOrDefault(a => a.AdminType.AdminTypeName == "Голова Куреня"
                    && (DateTime.Now < a.EndDate || a.EndDate == null));
            var ClubAdmins = Club.ClubAdministration
                .Where(a => a.AdminType.AdminTypeName != "Голова Куреня"
                    && (DateTime.Now < a.EndDate || a.EndDate == null))
                .ToList();

            var ClubProfileDto = new ClubProfileDTO
            {
                Club = Club,
                Admins = ClubAdmins,
                Head = ClubHead
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
            var logoBase64 = await _ClubBlobStorage.GetBlobBase64Async(logoName);

            return logoBase64;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(int ClubId)
        {
            var Club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == ClubId);

            if (Club.Logo != null)
            {
                await _ClubBlobStorage.DeleteBlobAsync(Club.Logo);
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
            await UploadPhotoAsync(model);
            var Club = CreateClubAsync(model);

            _repoWrapper.Club.Attach(Club);
            await _repoWrapper.Club.CreateAsync(Club);
            await _repoWrapper.SaveAsync();

            return Club.ID;
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

                await _ClubBlobStorage.UploadBlobForBase64Async(logoBase64Parts[1], fileName);
                club.Logo = fileName;
            }

            if (!string.IsNullOrEmpty(oldImageName))
            {
                await _ClubBlobStorage.DeleteBlobAsync(oldImageName);
            }
        }
    }
}
