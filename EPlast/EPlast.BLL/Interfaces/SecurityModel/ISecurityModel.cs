using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Interfaces
{
    public interface ISecurityModel
    {
        Dictionary<string, bool> GetUserAccess(string userId, IEnumerable<string> userRoles);
        void SetSettingsFile(string jsonPath);
    }
}
