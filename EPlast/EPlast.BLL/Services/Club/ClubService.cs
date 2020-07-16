using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;
using Microsoft.Azure.Storage.Blob;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Club
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IClubBlobStorageRepository _clubBlobStorage;

        public ClubService(IRepositoryWrapper repoWrapper, IMapper mapper, IWebHostEnvironment env, IClubBlobStorageRepository clubBlobStorage)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
            _clubBlobStorage = clubBlobStorage;
        }

        public async Task<IEnumerable<ClubDTO>> GetAllClubsAsync()
        {
            return _mapper.Map<IEnumerable<DataAccessClub.Club>, IEnumerable<ClubDTO>>(
                await _repoWrapper.Club.GetAllAsync());
        }

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

        public async Task<ClubDTO> GetClubInfoByIdAsync(int id)
        {
            var club = await _repoWrapper.Club.GetFirstOrDefaultAsync(c => c.ID == id);
            return (club != null)
                ? _mapper.Map<DataAccessClub.Club, ClubDTO>(club)
                : throw new ArgumentNullException($"Club with {id} id not found");
        }

        private async Task<ClubDTO> GetByIdWithDetailsAsync(int id)
        {
            var club = await _repoWrapper.Club
                .GetFirstOrDefaultAsync(
                    q => q.ID == id,
                    q =>
                        q.Include(c => c.ClubAdministration)
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

        private UserDTO GetCurrentClubAdmin(ClubDTO club)
        {
            return club?.ClubAdministration
                .Where(a => (a.EndDate >= DateTime.Now || a.EndDate == null) && a.AdminType.AdminTypeName == "Курінний")
                .Select(a => a.ClubMembers.User)
                .FirstOrDefault();
        }

        private IEnumerable<ClubAdministrationDTO> GetCurrentClubAdministration(ClubDTO club)
        {
            return club?.ClubAdministration
                .Where(a => a.EndDate >= DateTime.Now || a.EndDate == null)
                .ToList();
        }

        private IEnumerable<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved, int amount)
        {
            return club?.ClubMembers.Where(m => m.IsApproved == isApproved)
                .Take(amount)
                .ToList();
        }

        private IEnumerable<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved)
        {
            return club?.ClubMembers.Where(m => m.IsApproved == isApproved)
                .ToList();
        }

        public async Task UpdateAsync(ClubDTO club, IFormFile file)
        {
            var clubInfo = await GetClubInfoByIdAsync(club.ID);
            var oldImageName = clubInfo?.Logo;
            UpdateOrCreateAnImage(club, file, oldImageName);
            _repoWrapper.Club.Update(_mapper.Map<ClubDTO, DataAccessClub.Club>(club));
            await _repoWrapper.SaveAsync();
        }

        public async Task<ClubDTO> UpdateAsync(ClubDTO club)
        {
            await GetClubInfoByIdAsync(club.ID);
            var editedClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            _repoWrapper.Club.Update(editedClub);
            await _repoWrapper.SaveAsync();
            return _mapper.Map<DataAccessClub.Club, ClubDTO>(editedClub);
        }

        private void UpdateOrCreateAnImage(ClubDTO club, IFormFile file, string oldImageName = null)
        {
            if (file != null && file.Length > 0)
            {
                using (var img = Image.FromStream(file.OpenReadStream()))
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Club");
                    if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.jpg"))
                    {
                        var oldPath = Path.Combine(uploads, oldImageName);
                        if (File.Exists(oldPath))
                        {
                            File.Delete(oldPath);
                        }
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    club.Logo = fileName;
                }
            }
            else
            {
                club.Logo = oldImageName;
            }
        }

        public async Task<ClubDTO> CreateAsync(ClubDTO club, IFormFile file)
        {
            var newClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            newClub.Logo = Guid.NewGuid().ToString();
            //UpdateOrCreateAnImage(club, file);
            await _clubBlobStorage.UploadBlobAsync(file, newClub.Logo);
            await _repoWrapper.Club.CreateAsync(newClub);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<DataAccessClub.Club, ClubDTO>(newClub);
        }

        public async Task<ClubDTO> CreateAsync(ClubDTO club)
        {
            var newClub = _mapper.Map<ClubDTO, DataAccessClub.Club>(club);
            await _repoWrapper.Club.CreateAsync(newClub);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<DataAccessClub.Club, ClubDTO>(newClub);
        }

        public async Task<ClubProfileDTO> GetClubMembersOrFollowersAsync(int clubId, bool isApproved)
        {
            var club = await GetByIdWithDetailsAsync(clubId);
            var members = GetClubMembers(club, isApproved);
            var clubAdmin = GetCurrentClubAdmin(club);

            return isApproved
                ? new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Members = members }
                : new ClubProfileDTO { Club = club, ClubAdmin = clubAdmin, Followers = members };
        }

        public async Task<string> DownloadLogoFromBlobBase64Async(string fileName)
        {
            CloudBlockBlob blob = (await _clubBlobStorage.GetBlobAsync(fileName));
            blob.FetchAttributes();//Fetch blob's properties
            byte[] arr = new byte[blob.Properties.Length];
            blob.DownloadToByteArray(arr, 0);
            var azureBase64 = Convert.ToBase64String(arr);
            return azureBase64;
        }
    }
}