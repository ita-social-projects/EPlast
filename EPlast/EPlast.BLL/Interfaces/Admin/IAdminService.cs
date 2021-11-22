﻿using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">The id of the user</param>
        Task ChangeAsync(string userId);

        /// <summary>
        /// Change Current Role of user
        /// </summary>
        Task ChangeCurrentRoleAsync(string userId, string role);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">The id of the user, which must be deleted</param>
        Task DeleteUserAsync(string userId);

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="roles">List of new user roles</param>
        Task EditAsync(string userId, IEnumerable<string> roles);

        /// <summary>
        /// Get City and Region Admins by userId of user which contained cityMembers
        /// </summary>
        Task<IEnumerable<CityDTO>> GetCityRegionAdminsOfUserAsync(string userId);

        /// <summary>
        /// Get all roles except Admin role
        /// </summary>
        /// <returns>All roles except Admin role</returns>
        IEnumerable<IdentityRole> GetRolesExceptAdmin();

        /// <summary>
        /// Get all users with additional information
        /// </summary>
        /// <returns>Specify model with all users</returns>
        Task<Tuple<IEnumerable<UserTableDTO>, int>> GetUsersTableAsync(TableFilterParameters tableFilterParameters);

        /// <summary>
        /// Gets short users infos, by search string
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <returns>Users that match search string</returns>
        Task<IEnumerable<ShortUserInformationDTO>> GetShortUserInfoAsync(string searchString);

        Task<IEnumerable<ShortUserInformationDTO>> GetUsersAsync();

        Task<int> GetUsersCountAsync();

        /// <summary>
        /// Get all users that match the condition and roles in delegate
        /// </summary>
        /// <param name="rolesString">String that represents roles grouprs divided by '|' and roles by ','</param>
        /// <param name="include">Boolean variable that indicates whether include or exclude user by roles</param>
        /// <returns>Users that have the roles</returns>
        Task<IEnumerable<ShortUserInformationDTO>> GetUsersByRolesAsync(string rolesString, bool include, Func<IEnumerable<User>, IEnumerable<string>, bool, Task<IEnumerable<ShortUserInformationDTO>>> filterRoles);

        /// <summary>
        /// Get users which have any role of the specified roles
        /// </summary>
        Task<IEnumerable<ShortUserInformationDTO>> FilterByAnyRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include);

        /// <summary>
        /// Get users which have all the specified roles
        /// </summary>
        Task<IEnumerable<ShortUserInformationDTO>> FilterByAllRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include);

        /// <summary>
        /// Get users which have only the specified roles
        /// </summary>
        Task<IEnumerable<ShortUserInformationDTO>> FilterByExactRoles(IEnumerable<User> users, IEnumerable<string> roles, bool include);
    }
}
