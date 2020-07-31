using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    /// <summary>
    /// Implement  operations for work with active membership
    /// </summary>
    public interface IActiveMembershipService
    {
       /// <summary>
       /// Returns all plast degrees
       /// </summary>
       /// <returns>All plast degrees</returns>
        public Task<IEnumerable<PlastDergeeDTO>> GetDergeesAsync();
    }
}
