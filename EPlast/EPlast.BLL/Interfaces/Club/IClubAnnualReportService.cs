using EPlast.BLL.DTO.Club;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAnnualReportService
    {
        /// <summary>
        /// Method to get all the information in the club annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report model</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task<ClubAnnualReportDTO> GetByIdAsync(User user, int id);

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <returns>List of annual report model</returns>
        Task<IEnumerable<ClubAnnualReportDTO>> GetAllAsync(User user);

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="searchedData">Authorized user</param>
        /// <param name="page">Authorized user</param>
        /// <param name="pageSize">Authorized user</param>
        /// <returns>List of ClubAnnualReportTableObjectl</returns>
        Task<IEnumerable<ClubAnnualReportTableObject>> GetAllAsync(User user, string searchedData, int page, int pageSize);

        /// <summary>
        /// Method to create new club annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="clubAnnualReportDTO">Annual report model</param>
        /// <exception cref="System.InvalidOperationException">Thrown when city has created annual report</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to city</exception>
        /// <exception cref="System.NullReferenceException">Thrown when city doesn't exist</exception>
        Task CreateAsync(User user, ClubAnnualReportDTO clubAnnualReportDTO);

        /// <summary>
        /// Method to confirm club annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="id">Annual report identification number</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to annual report</exception>
        /// <exception cref="System.NullReferenceException">Thrown when annual report doesn't exist</exception>
        Task ConfirmAsync(User user, int id);

        Task CancelAsync(User user, int id);
        Task DeleteClubReportAsync(User user, int id);
        Task EditClubReportAsync(User user, ClubAnnualReportDTO clubAnnualReportDto);
    }
}
