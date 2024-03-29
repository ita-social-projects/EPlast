﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportService
    {
        /// <summary>
        /// Method to get all the information in the annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<AnnualReportDto> GetByIdAsync(User user, int id);

        /// <summary>
        /// Method to get data for the annual report edit form
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<AnnualReportDto> GetEditFormByIdAsync(User user, int id);

        /// <summary>
        /// Method to get all reports that the user has access to
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <returns>List of annual report model</returns>
        Task<IEnumerable<AnnualReportDto>> GetAllAsync(User user);

        /// <summary>
        /// Method to get all searched reports that the user has access to
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="isAdmin">Whether authorized user is admin</param>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">current page on pagination</param>
        /// <param name="pageSize">number of records per page</param>
        /// <param name="sortKey">Key for sorting</param>
        /// <param name="auth">Whether to select reports of that user is author</param>
        /// <returns>List of AnnualReportTableObject</returns>
        Task<IEnumerable<AnnualReportTableObject>> GetAllAsync(User user, bool isAdmin, string searchedData, int page,
            int pageSize, int sortKey, bool auth);

        /// <summary>
        /// Method to create new annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="annualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when city has created annual report</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to city</exception>
        /// <exception cref="System.NullReferenceException">Thrown when city doesn't exist</exception>
        Task CreateAsync(User user, AnnualReportDto annualReportDTO);

        /// <summary>
        /// Get a list of members of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A list of members of a specific city</returns>
        Task<CityDto> GetCityMembersAsync(int cityId);

        /// <summary>
        /// Method to edit annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="annualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when annual report can not be edited</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task EditAsync(User user, AnnualReportDto annualReportDTO);

        /// <summary>
        /// Method to confirm annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task ConfirmAsync(User user, int id);

        /// <summary>
        /// Method to cancel annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task CancelAsync(User user, int id);

        /// <summary>
        /// Method to delete annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task DeleteAsync(User user, int id);

        /// <summary>
        /// Method to check whether city has created annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="cityId">City identification number</param>
        /// <returns>Information whether city has created annual report</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to city</exception>
        /// <exception cref="System.NullReferenceException">Thrown when city doesn't exist</exception>
        Task<bool> CheckCreated(User user, int cityId);
    }
}