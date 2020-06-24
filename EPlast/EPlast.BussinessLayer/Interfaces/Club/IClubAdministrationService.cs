using System;
using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO.Club;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetClubAdministrationByIdAsync(int clubId);
        Task<bool> DeleteClubAdminAsync(int id);
        Task<ClubAdministrationDTO> SetAdminEndDateAsync(int clubAdministrationId, DateTime endDate);
        Task<ClubAdministrationDTO> AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
