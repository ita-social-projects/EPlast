using EPlast.BLL.DTO.Club;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using System;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAccessService
    {
        Task<IEnumerable<ClubDTO>> GetClubsAsync(User user);
        Task<IEnumerable<Tuple<int, string>>> GetAllClubsIdAndName(User user);
        Task<bool> HasAccessAsync(User user, int ClubId);
        Task<bool> HasAccessAsync(User user);
    }
}
