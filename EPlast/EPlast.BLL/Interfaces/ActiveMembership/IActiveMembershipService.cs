using EPlast.BLL.DTO.ActiveMembership;
using System;
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
        public Task<IEnumerable<PlastDegreeDTO>> GetDergeesAsync();
        /// <summary>
        /// Returns user date of entry
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User date of entry</returns>
        public Task<DateTime> GetDateOfEntryAsync(string userId);
        /// <summary>
        /// Returns All degrees of current user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>All degrees of current user</returns>
        public Task<IEnumerable<UserPlastDegreeDTO>> GetUserPlastDegreesAsync(string userId);
        /// <summary>
        /// Returns All access levels of current user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>All access levels of current user</returns>
        public Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId);
        /// <summary>
        /// Adds plast degree for user
        /// </summary>
        /// <param name="userPlastDegreePostDTO"></param>
        /// <returns>A bool value that says that degree was added</returns>
        public Task<bool> AddPlastDegreeForUserAsync(UserPlastDegreePostDTO userPlastDegreePostDTO);
        /// <summary>
        /// Removes user degree
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="plastDegreeId">Plast degree id</param>
        /// <returns>A bool value that says that degree was deleted</returns>
        public Task<bool> DeletePlastDegreeForUserAsync(string userId, int plastDegreeId);
        /// <summary>
        /// Adds end date for user plast degree
        /// </summary>
        /// <param name="userPlastDegreePutDTO">User plast degree put dto</param>
        /// <returns>A bool value that says that end date was added</returns>
        public Task<bool> AddEndDateForUserPlastDegreeAsync(UserPlastDegreePutDTO userPlastDegreePutDTO);
        /// <summary>
        /// Sets plast degree for user as current
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="plastDegreeId">Plast degree id</param>
        /// <returns>A bool value that says that degree  was changed to current</returns>
        public Task<bool> SetPlastDegreeForUserAsCurrentAsync(string userId, int plastDegreeId);
    }
}
