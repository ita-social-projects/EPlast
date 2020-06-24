using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityDTO>> GetAllAsync();
    }
}
