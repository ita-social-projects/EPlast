using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserAccess
{
    public interface IUserAccessService
    {
        /// <summary>
        /// Returns dictionary with user accesses for clubs
        /// </summary>
        Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId, string userId, User user);

        /// <summary>
        /// Returns dictionary with user accesses for distinctions
        /// </summary>
        Task<Dictionary<string, bool>> GetUserDistinctionAccessAsync(string userId, User user);
    }
}