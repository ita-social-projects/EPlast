using System.Collections.Generic;

namespace EPlast.BLL.Interfaces
{
    public interface ISecurityModel
    {
        Dictionary<string, bool> GetUserAccess(string userId, IEnumerable<string> userRoles= null);
        void SetSettingsFile(string jsonFileName);
    }
}
