using EPlast.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodiesListAsync();

        Task<int> CreateAsync(GoverningBodyDTO governingBodyDto);

        Task<string> GetLogoBase64Async(string logoName);

        Task<GoverningBodyProfileDTO> GetProfileAsync(int id, User user);

        Task<int> RemoveAsync(int governingBodyId);

        Task<int> EditAsync(GoverningBodyDTO governingBody);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);
    }
}
