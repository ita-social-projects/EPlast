using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface ISecurityModel
    {
        Task<Dictionary<string, bool>> GetUserAccess(string userId, IEnumerable<string> userRoles= null);
        void SetSettingsFile(string jsonFileName);
    }
}
