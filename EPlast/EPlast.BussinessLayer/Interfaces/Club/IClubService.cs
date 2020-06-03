using EPlast.BussinessLayer.DTO.Club;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubService
    {
        IEnumerable<ClubDTO> GetAllClubs();
        ClubProfileDTO GetClubProfile(int clubId);
        ClubDTO GetClubInfoById(int id);
        ClubProfileDTO GetClubMembersOrFollowers(int clubId, bool isApproved);
        void Update(ClubDTO club, IFormFile file);
        ClubDTO Create(ClubDTO club, IFormFile file);
    }
}