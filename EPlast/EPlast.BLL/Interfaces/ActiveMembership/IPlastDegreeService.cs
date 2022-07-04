using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;

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
        public Task<IEnumerable<PlastDegreeDto>> GetDegreesAsync();
        /// <summary>
        /// Check allowed degrees
        /// </summary>
        /// <param name="degreeName">Degree name</param>
        /// <returns>A bool value that says if there is such degree at the  list of degrees</returns>
        public Task<bool> CheckDegreeAsync(int degreeId, List<string> appropriateDegrees);
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
        public Task<UserPlastDegreeDto> GetUserPlastDegreeAsync(string userId);
        /// <summary>
        /// Adds plast degree for user
        /// </summary>
        /// <param name="userPlastDegreePostDTO"></param>
        /// <returns>A bool value that says that degree was added</returns>
        public Task<bool> AddPlastDegreeForUserAsync(UserPlastDegreePostDto userPlastDegreePostDTO);
        /// <summary>
        /// Removes user degree
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="plastDegreeId">Plast degree id</param>
        /// <returns>A bool value that says that degree was deleted</returns>
        public Task<bool> DeletePlastDegreeForUserAsync(string userId, int plastDegreeId);
    }
}
