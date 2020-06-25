using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        Task<ClubProfileDTO> GetCurrentClubAdministrationByIDAsync(int clubID);
        Task<bool> DeleteClubAdminAsync(int id);
        Task SetAdminEndDateAsync(AdminEndDateDTO adminEndDate);
        Task AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}
