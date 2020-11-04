using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAnnualReportService
    {
        /// <summary>
        /// Method to get all the information in the club annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<RegionAnnualReportDTO> GetByIdAsync(ClaimsPrincipal claimsPrincipal, int id);

        /// <summary>
        /// Method to get all region reports that the user has access to
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <returns>List of annual report model</returns>
        Task<IEnumerable<RegionAnnualReportDTO>> GetAllAsync(ClaimsPrincipal claimsPrincipal);

        /// <summary>
        /// Method to create new region annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="regionAnnualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when region has created annual report</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to region</exception>
        /// <exception cref="System.NullReferenceException">Thrown when region doesn't exist</exception>
        Task CreateAsync(ClaimsPrincipal claimsPrincipal, RegionAnnualReportDTO regionAnnualReportDTO);
    }
}