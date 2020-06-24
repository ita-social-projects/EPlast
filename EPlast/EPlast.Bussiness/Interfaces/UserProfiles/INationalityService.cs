using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.UserProfiles
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityDTO>> GetAllAsync();
    }
}
