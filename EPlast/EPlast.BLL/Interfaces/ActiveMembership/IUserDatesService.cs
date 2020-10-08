using EPlast.BLL.DTO.ActiveMembership;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.ActiveMembership
{
    /// <summary>
    /// Implement  operations for work with access levels
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
        /// <param name="userMembershipDatesDTO"> userMembershipDatesDTO</param>
        /// <returns>bool if changing is successful</returns>
        public Task<bool> ChangeUserMembershipDatesAsync(UserMembershipDatesDTO userMembershipDatesDTO);

        public Task<bool> AddDateEntryAsync(string userId);
    }


}
