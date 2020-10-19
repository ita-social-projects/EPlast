using EPlast.BLL.DTO.Club;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAccessService
    {
        Task<IEnumerable<ClubDTO>> GetClubsAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int ClubId);
    }
}
