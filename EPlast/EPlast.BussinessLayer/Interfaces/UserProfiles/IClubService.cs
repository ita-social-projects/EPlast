using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EPlast.BussinessLayer.DTO;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IClubService
    {
        IEnumerable<ClubDTO> GetAllClubs();
        ClubProfileDTO GetClubProfile(int clubId);
        ClubDTO GetByIdWithDetails(int id);
        ClubDTO GetById(int id);
        List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved, int amount);
        List<ClubMembersDTO> GetClubMembers(ClubDTO club, bool isApproved);
        ClubProfileDTO GetClubMembersOrFollowers(int clubId, bool isApproved);
        UserDTO GetCurrentClubAdmin(ClubDTO club);
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