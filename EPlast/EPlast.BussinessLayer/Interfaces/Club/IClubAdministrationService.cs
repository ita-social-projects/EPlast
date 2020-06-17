using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetCurrentClubAdministrationByIDAsync(int clubID);
        Task<bool> DeleteClubAdminAsync(int id);
        Task SetAdminEndDateAsync(AdminEndDateDTO adminEndDate);
        Task AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
