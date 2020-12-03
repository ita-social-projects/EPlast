using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAnnualReportService
    {
        /// <summary>
        /// Method to get all the information in the region annual report
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <param name="year">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<RegionAnnualReportDTO> CreateByNameAsync(ClaimsPrincipal claimsPrincipal, int id, int year, RegionAnnualReportQuestions regionAnnualReportQuestions);

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

        /// <summary>
        /// Method to get region report by Id
        /// </summary>
        /// <param name="claimsPrincipal">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <param name="year">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<RegionAnnualReportDTO> GetReportByIdAsync(int id,int year);


        /// <summary>
        /// Method to get all regions reports
        /// </summary>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<IEnumerable<RegionAnnualReportDTO>> GetAllRegionsReportsAsync();
    }
}