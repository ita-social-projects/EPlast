using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using EPlast.DataAccess.DTO;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IClubService
    {
        List<Club> GetAllClubs();
        Club GetByIdWithDetails(int id);
        Club GetById(int id);
        List<ClubMembers> GetClubMembers(Club club, bool isApproved, int amount);
        List<ClubMembers> GetClubMembers(Club club, bool isApproved);
        User GetCurrentClubAdmin(Club club);
        void Update(Club club, IFormFile file);
        void Create(Club club, IFormFile file);
        void ToggleIsApprovedInClubMembers(int memberId, int clubId);
        bool DeleteClubAdmin(int id);
        void SetAdminEndDate(AdminEndDateDTO adminEndDate);
        void AddClubAdmin(ClubAdministrationDTO createdAdmin);
        void AddFollower(int index, string userId);
    }
}
