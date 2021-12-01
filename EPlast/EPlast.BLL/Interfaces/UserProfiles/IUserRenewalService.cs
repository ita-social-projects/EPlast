using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IUserRenewalService
    {
        /// <summary>
        /// Adds new Former-Member renewal request
        /// </summary>
        /// <param name="userRenewal">UserRenewal(dto) contains the renewal information</param>
        Task AddUserRenewalAsync(UserRenewalDTO userRenewal);
        
        /// <summary>
        /// Makes changes to the Former-Member renewal request status
        /// </summary>
        /// <param name="userRenewal">UserRenewal(dto) contains the renewal information</param>
        Task ChangeUserRenewalAsync(UserRenewalDTO userRenewal);

        /// <summary>
        /// Identifies whether target Former-Member renewal request is valid
        /// </summary>
        /// <param name="userRenewal">UserRenewal(dto) contains the renewal information</param>
        /// <returns>Is this Former-Member renewal request valid</returns>
        Task<bool> IsValidUserRenewalAsync(UserRenewalDTO userRenewal);
        
        /// <summary>
        /// Identifies whether the user can renew Former-Member in specified city
        /// </summary>
        /// <param name="user">User (admin) who tries to renew Former-Member</param>
        /// <param name="cityId">The id of the city where Former-Member wil be renewed</param>
        /// <returns>Is this user(admin) valid to renew the Former-Member</returns>
        Task<bool> IsValidAdminAsync(User user, int cityId);

        /// <summary>
        /// Returns UserRenewals with params
        /// </summary>
        /// <param name="searchedData">Search string</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>UserRenewals</returns>
        IEnumerable<UserRenewalsTableObject> GetUserRenewalsTableObject(string searchedData, int page, int pageSize);

        /// <summary>
        /// Renews Former-Member as registered user, adds him to the members of chosen city
        /// </summary>
        /// <param name="userRenewal">UserRenewal(dto) contains the renewal information</param>
        /// <returns>Information about new city member</returns>
        Task<CityMembersDTO> RenewFormerMemberUserAsync(UserRenewalDTO userRenewal);

        /// <summary>
        /// Sends confirmation email for renewed Former-Member users
        /// </summary>
        /// <param name="userId">The id of the user who's gotten renewed recently</param>
        /// <param name="cityId">The id of the city where user has been renewed</param>
        Task SendRenewalConfirmationEmailAsync(string userId, int cityId);

        /// <summary>
        /// Resets user membership dates, makes renewed user as if he's been just registered.
        /// For UserRenewalService internal use only
        /// </summary>
        /// <param name="userId">The id of the user who's being renewed</param>
        Task ResolveUserMembershipDatesAsync(string userId);
    }
}
