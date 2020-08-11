using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    /// <summary>
    /// Implement  operations for work with access levels
    /// </summary>
    public interface IAccessLevelService
    {
        /// <summary>
        /// Returns All access levels of current user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>All access levels of current user</returns>
        public Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId);
    }
}
