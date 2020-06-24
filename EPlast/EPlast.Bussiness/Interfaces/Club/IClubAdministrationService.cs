using EPlast.Bussiness.DTO;
using EPlast.Bussiness.DTO.Club;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetCurrentClubAdministrationByIDAsync(int clubID);
        Task<bool> DeleteClubAdminAsync(int id);
        Task SetAdminEndDateAsync(AdminEndDateDTO adminEndDate);
        Task AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
