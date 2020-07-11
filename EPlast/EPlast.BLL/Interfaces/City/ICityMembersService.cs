using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId);
        Task<CityMembersDTO> AddCityFollower(int cityId, string userId);
        Task<CityMembersDTO> ToggleMemberStatus(int cityId, string userId);
        Task RemoveMember(string userId);
    }
}