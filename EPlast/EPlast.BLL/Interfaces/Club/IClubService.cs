using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubService
    {
        Task<IEnumerable<ClubDTO>> GetAllClubsAsync();

        Task<ClubProfileDTO> GetClubProfileAsync(int clubId);

        Task<ClubDTO> GetClubInfoByIdAsync(int id);

        Task<ClubProfileDTO> GetClubMembersOrFollowersAsync(int clubId, bool isApproved);

        Task UpdateAsync(ClubDTO club, IFormFile file);

        Task<ClubDTO> UpdateAsync(ClubDTO club);

        Task<ClubDTO> CreateAsync(ClubDTO club, IFormFile file);

        Task<ClubDTO> CreateAsync(ClubDTO club);

        Task<string> DownloadLogoFromBlobBase64Async(string fileName);
    }
}