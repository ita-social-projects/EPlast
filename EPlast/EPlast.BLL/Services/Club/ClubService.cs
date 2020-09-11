using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Club
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IClubBlobStorageRepository _clubBlobStorage;

        public ClubService(IRepositoryWrapper repoWrapper, IMapper mapper, IClubBlobStorageRepository clubBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _clubBlobStorage = clubBlobStorage;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubDTO>> GetAllClubsAsync()
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(
                await _repoWrapper.Club.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubDTO>> GetPartOfClubsAsync( int pageNumber, int pageSize)
        {
            var sampleOfClubs = await _repoWrapper.Club.FindAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedClubs = _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(sampleOfClubs);

            return mappedClubs;
        }

        /// <inheritdoc />
        public async Task<int> GetClubsCountAsync()
        {
            var clubsCount = await _repoWrapper.Club.FindAll().CountAsync();

            return clubsCount;
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubProfileAsync(int clubId)
        {
            var club = await GetByIdWithDetailsAsync(clubId);
            var members = GetClubMembers(club, true, 6);
            var followers = GetClubMembers(club, false, 6);
            var clubAdministration = GetCurrentClubAdministration(club);
            var clubAdmin = GetCurrentClubAdmin(club);

            return new ClubProfileDTO
            {
                Club = club,
                Members = members,
                Followers = followers,
                ClubAdmin = clubAdmin,
                ClubAdministration = clubAdministration
            };
        }

        /// <inheritdoc />
        public async Task<ClubDTO> GetClubInfoByIdAsync(int id)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == id);
            return (club != null)
                ? _mapper.Map<DataAccessClub.Club, ClubDTO>(club)
                : throw new ArgumentNullException($"Club with {id} id not found");
        }

        private async Task<ClubDTO> GetByIdWithDetailsAsync(int id)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(q => q.ID == id,
                q => q.Include(c => c.ClubAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(n => n.ClubAdministration)
                    .ThenInclude(t => t.ClubMembers)
                    .ThenInclude(us => us.User)
                    .Include(m => m.ClubMembers)
                    .ThenInclude(u => u.User));
            return (club != null)
                ? _mapper.Map<DataAccessClub.Club, ClubDTO>(club)
                : throw new ArgumentNullException($"Club with {id} id not found");
        }

        private static UserDTO GetCurrentClubAdmin(ClubDTO club)
        {
            return club?.ClubAdministration
                .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                .Select(a => a.ClubMembers.User)
                .FirstOrDefault();
        }

        private static IEnumerable<ClubAdministrationDTO> GetCurrentClubAdministration(ClubDTO club)
        {
            return club?.ClubAdministration.Where(a => a.EndDate >= DateTime.Now || a.EndDate == null).ToList();
        }

        private static IEnumerable<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved, int amount)
        {
            return club?.ClubMembers.Where(m => m.IsApproved == isApproved).Take(amount).ToList();
        }

        private static IEnumerable<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved)
        {
            return club?.ClubMembers.Where(m => m.IsApproved == isApproved).ToList();
        }

        /// <inheritdoc />
        public async Task<ClubDTO> UpdateAsync(ClubDTO club)
        {
            var editedClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            editedClub.Logo = await UploadPhotoAsyncFromBase64(club.ID, club.Logo);
            _repoWrapper.Club.Update(editedClub);
            await _repoWrapper.SaveAsync();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(editedClub);
        }

        /// <inheritdoc />
        public async Task<ClubDTO> CreateAsync(ClubDTO club)
        {
            var newClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            await _repoWrapper.Club.CreateAsync(newClub);
            await _repoWrapper.SaveAsync();
            newClub.Logo = await UploadPhotoAsyncFromBase64(newClub.ID, newClub.Logo);
            _repoWrapper.Club.Update(newClub);
            await _repoWrapper.SaveAsync();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(newClub);
        }

        /// <inheritdoc />
        public async Task<ClubProfileDTO> GetClubMembersOrFollowersAsync(int clubId, bool isApproved)
        {
            var club = await GetByIdWithDetailsAsync(clubId);
            var members = GetClubMembers(club, isApproved);
            var clubAdmin = GetCurrentClubAdmin(club);

            return isApproved
                ? new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Members = members }
                : new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Followers = members };
        }

        private async Task<string> UploadPhotoAsyncFromBase64(int clubId, string imageBase64)
        {
            var oldImageName = (await _repoWrapper.Club.GetFirstOrDefaultAsync(x => x.ID == clubId)).Logo;
            if (string.IsNullOrWhiteSpace(imageBase64) || imageBase64.Length <= 0) return oldImageName;
            var base64Parts = imageBase64.Split(',');
            var ext = base64Parts[0].Split(new[] { '/', ';' }, 3)[1];
            var fileName = Guid.NewGuid() + "." + ext;
            await _clubBlobStorage.UploadBlobForBase64Async(base64Parts[1], fileName);
            if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default_club_image.png") && oldImageName.Length < 30)
            {
                await _clubBlobStorage.DeleteBlobAsync(oldImageName);
            }

            return fileName;
        }

        /// <inheritdoc />
        public async Task<string> GetImageBase64Async(string fileName)
        {
            return await _clubBlobStorage.GetBlobBase64Async(string.IsNullOrEmpty(fileName)
                ? "default_club_image.png"
                : fileName);
        }

        public async Task<bool> ValidateAsync(ClubDTO club)
        {
            var isClubNameNotUnique = await _repoWrapper.Club.FindAll().AnyAsync(x => x.ClubName == club.ClubName);

            return !isClubNameNotUnique;
        }

        public async Task<bool> VerifyClubNameIsNotChangedAsync(ClubDTO club)
        {
            var originClub = await _repoWrapper.Club.GetFirstOrDefaultAsync(x => x.ID == club.ID);

            var isTheSameClubName = originClub.ClubName == club.ClubName;

            return isTheSameClubName;
        }

        public async Task<bool> VerifyUserCanJoinToClubAsync(int clubId, string userId)
        {
            var clubMember = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.User.Id == userId && u.ClubId == clubId);
            var canJoin = clubMember == null;

            return canJoin;
        }
    }
}