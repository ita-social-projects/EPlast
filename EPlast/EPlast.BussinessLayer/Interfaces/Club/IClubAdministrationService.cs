using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;

namespace EPlast.BussinessLayer.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        ClubProfileDTO GetCurrentClubAdministrationByID(int clubID);
        bool DeleteClubAdmin(int id);
        void SetAdminEndDate(AdminEndDateDTO adminEndDate);
        void AddClubAdmin(ClubAdministrationDTO createdAdmin);
    }
}
