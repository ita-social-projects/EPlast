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

        Task<string> GetLogoBase64(string logoName);

        Task<GoverningBodyProfileDTO> GetProfileById(int id, User user);

        Task RemoveAsync(int governingBodyId);

        Task EditAsync(GoverningBodyDTO governingBody);
    }
}
