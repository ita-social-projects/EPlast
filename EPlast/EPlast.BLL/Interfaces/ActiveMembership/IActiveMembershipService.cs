using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    public interface IActiveMembershipService
    {
        public Task<IEnumerable<PlastDergeeDTO>> GetDergeesAsync();
    }
}
