using EPlast.BLL.DTO.ActiveMembership;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    /// <summary>
    /// Implement  operations for work with plast degrees
    /// </summary>
    public interface IPlastDegreeService
    {
        /// <summary>
        /// Returns all plast degrees
        /// </summary>
        /// <returns>All plast degrees</returns>
        public Task<IEnumerable<PlastDegreeDTO>> GetDergeesAsync();
        /// <summary>
        /// Check allowed degrees
        /// </summary>
        /// <param name="degreeName">Degree name</param>
        /// <returns>A bool value that says that degree contains</returns>
        public Task<bool> GetDergeeAsync(int degreeId, List<string> appropriateDegrees);
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
