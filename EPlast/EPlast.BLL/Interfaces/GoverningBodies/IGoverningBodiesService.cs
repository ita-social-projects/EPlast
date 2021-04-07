using EPlast.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync();
    }
}
