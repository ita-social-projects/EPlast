using EPlast.BusinessLogicLayer.DTO.Admin;
using EPlast.BusinessLogicLayer.DTO.Club;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetCurrentClubAdministrationByIDAsync(int clubID);
        Task<bool> DeleteClubAdminAsync(int id);
        Task SetAdminEndDateAsync(AdminEndDateDTO adminEndDate);
        Task AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
