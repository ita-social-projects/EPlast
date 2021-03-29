﻿using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IUserService
    {
        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>A user dto</returns>
        Task<UserDTO> GetUserAsync(string userId);
        Task UpdateAsyncForFile(UserDTO user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId);

        /// <summary>
        /// Change a user
        /// </summary>
        /// <param name="user">User(dto) which needs to be changed</param>
        /// <param name="base64">Image in base64 format, which must be saved</param>
        /// <param name="placeOfStudyId">Id of "Education" which contains chosen place of study</param>
        /// <param name="specialityId">Id of "Education" which contains chosen speciality</param>
        /// <param name="placeOfWorkId">Id of "Work" which contains chosen place of work</param>
        /// <param name="positionId">Id of "Work" which contains chosen position</param>
        Task UpdateAsyncForBase64(UserDTO user, string base64, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId);

        /// <summary>
        /// Checks whether the user is registered more than a year ago, if yes - then provides the role of "Plastun"
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="registeredOn">User registration date</param>
        /// <returns>The time left before the role of "Plastun"</returns>
        TimeSpan CheckOrAddPlastunRole(string userId, DateTime registeredOn);

        /// <summary>
        /// Get confirmed users
        /// </summary>
        /// <param name="user">User(dto) which contains confirmed users</param>
        /// <returns>Obtaining a "confirmed users" of the selected user</returns>
        IEnumerable<ConfirmedUserDTO> GetConfirmedUsers(UserDTO user);

        /// <summary>
        /// Get confirmed users which have access "Club admin"
        /// </summary>
        /// <param name="user">User(dto) which contains confirmed users</param>
        /// <returns>Obtaining a "confirmed users" of the selected user</returns>
        ConfirmedUserDTO GetClubAdminConfirmedUser(UserDTO user);

        /// <summary>
        /// Get confirmed users which have access "City admin"
        /// </summary>
        /// <param name="user">User(dto) which contains confirmed users</param>
        /// <returns>Obtaining a "confirmed users" of the selected user</returns>
        ConfirmedUserDTO GetCityAdminConfirmedUser(UserDTO user);

        /// <summary>
        /// Whether the user can confirm the selected user
        /// </summary>
        /// <param name="confUsers">List of confirmed user(dto) which contains in selected user</param>
        /// <param name="userId">The id of the selected user</param>
        /// <param name="user">Authorized user</param>
        /// <returns>Can the user approve</returns>
        bool CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, User user);


        /// <summary>
        /// Get a image
        /// </summary>
        /// <param name="fileName">The name of the image</param>
        /// <returns>Image in format base64</returns>
        Task<string> GetImageBase64Async(string fileName);

        Task<bool> IsApprovedCityMember(string userId);
    }
}
