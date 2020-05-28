using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IClubService
    {
        IEnumerable<ClubDTO> GetAllClubs();
        ClubProfileDTO GetClubProfile(int clubId);
        ClubDTO GetById(int id);
        ClubProfileDTO GetClubMembersOrFollowers(int clubId, bool isApproved);
        ClubProfileDTO GetCurrentClubAdminByID(int clubID);
        void Update(ClubDTO club, IFormFile file);
        ClubDTO Create(ClubDTO club, IFormFile file);
        void ToggleIsApprovedInClubMembers(int memberId, int clubId);
        bool DeleteClubAdmin(int id);
        void SetAdminEndDate(AdminEndDateDTO adminEndDate);
        void AddClubAdmin(ClubAdministrationDTO createdAdmin);
        void AddFollower(int index, string userId);
    }
}