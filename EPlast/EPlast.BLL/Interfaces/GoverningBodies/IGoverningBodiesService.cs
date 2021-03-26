using EPlast.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodiesListAsync();

        Task<int> CreateAsync(GoverningBodyDTO governingBodyDto);

        Task<string> GetLogoBase64(string logoName);
    }
}
