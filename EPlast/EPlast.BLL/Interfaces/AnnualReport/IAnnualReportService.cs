using EPlast.BLL.DTO.AnnualReport;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAnnualReportService
    {
        /// <summary>
        /// Method to get all the information in the annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<AnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id);

        /// <summary>
        /// Method to get all reports that the user has access to
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <returns>List of annual report model</returns>
        Task<IEnumerable<AnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal);

        /// <summary>
        /// Method to create new annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="annualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when city has created annual report</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to city</exception>
        /// <exception cref="System.NullReferenceException">Thrown when city doesn't exist</exception>
        Task CreateAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO);

        /// <summary>
        /// Method to edit annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="annualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when annual report can not be edited</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task EditAsync(ClaimsPrincipal claimsPrincipal, AnnualReportDTO annualReportDTO);

        /// <summary>
        /// Method to confirm annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task ConfirmAsync(ClaimsPrincipal claimsPrincipal, int id);

        /// <summary>
        /// Method to cancel annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task CancelAsync(ClaimsPrincipal claimsPrincipal, int id);

        /// <summary>
        /// Method to delete annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task DeleteAsync(ClaimsPrincipal claimsPrincipal, int id);

        /// <summary>
        /// Method to check whether city has created annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="cityId">City identification number</param>
        /// <returns>Information whether city has created annual report</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to city</exception>
        /// <exception cref="System.NullReferenceException">Thrown when city doesn't exist</exception>
        Task<bool> CheckCreated(ClaimsPrincipal claimsPrincipal, int cityId);
    }
}