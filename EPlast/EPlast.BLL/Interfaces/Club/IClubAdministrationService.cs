using System;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetClubAdministrationByIdAsync(int clubId);
        Task<bool> DeleteClubAdminAsync(int id);
        Task<ClubAdministrationDTO> SetAdminEndDateAsync(int clubAdministrationId, DateTime endDate);
        Task<ClubAdministrationDTO> AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
