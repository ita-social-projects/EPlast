using EPlast.BLL.DTO.ActiveMembership;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    /// <summary>
    /// Implement  operations for work with user dates
    /// </summary>
    public interface IUserDatesService
    {
        /// <summary>
        /// Returns user dates like Date of entry into a Plast, date of oath and date of finish membership
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>All dates of current user</returns>
        public Task<UserMembershipDatesDTO> GetUserMembershipDatesAsync(string userId);

        /// <summary>
        /// Returns boolean, if dates is correct and they was changed in DB return true, else false
        /// </summary>
        /// <param name="entryAndOathDateDTO"> entryAndOathDateDTO</param>
        /// <returns>bool if changing is successful</returns>
        public Task<bool> ChangeUserEntryAndOathDateAsync(EntryAndOathDateDTO entryAndOathDateDTO);

        /// <summary>
        /// Returns boolean, if dates is correct and they was added to DB return true, else false
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>bool if adding is successful</returns>
        public Task<bool> AddDateEntryAsync(string userId);

        /// <summary>
        /// Returns boolean, if user has membership return true, else false
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>true if user has membership</returns>
        public Task<bool> UserHasMembership(string userId);

        /// <summary>
        /// Changes user memberships end dates to now
        /// </summary>
        /// <param name="userId">User id</param>
        public Task EndUserMembership(string userId);
    }


}
