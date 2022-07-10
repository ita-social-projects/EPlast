using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAccessService
    {
        Task<IEnumerable<ClubDto>> GetClubsAsync(User user);
        Task<IEnumerable<ClubForAdministrationDto>> GetAllClubsIdAndName(User user);
        Task<bool> HasAccessAsync(User user, int ClubId);
        Task<bool> HasAccessAsync(User user);
    }
}
