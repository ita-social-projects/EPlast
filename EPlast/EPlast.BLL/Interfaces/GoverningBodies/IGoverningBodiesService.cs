using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodiesListAsync();

        Task<int> CreateAsync(GoverningBodyDTO governingBodyDto);

        Task<string> GetLogoBase64Async(string logoName);

        Task<GoverningBodyProfileDTO> GetGoverningBodyProfileAsync(int governingBodyId);

        Task<int> RemoveAsync(int governingBodyId);

        Task<int> EditAsync(GoverningBodyDTO governingBody);

        Task<GoverningBodyProfileDTO> GetGoverningBodyDocumentsAsync(int governingBodyId);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);
    }
}
