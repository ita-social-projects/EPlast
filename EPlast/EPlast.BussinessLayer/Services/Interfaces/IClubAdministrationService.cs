using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.DTO;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IClubAdministrationService
    {
        UserDTO GetCurrentClubAdmin(ClubDTO club);
        bool DeleteClubAdmin(int id);
        void SetAdminEndDate(AdminEndDateDTO adminEndDate);
        void AddClubAdmin(ClubAdministrationDTO createdAdmin);
    }
}
