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
        /// Method to get all searched club reports that the user has access to
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="isAdmin">Whether authorized user is admin</param>
        /// <param name="searchedData">Authorized user</param>
        /// <param name="page">Authorized user</param>
        /// <param name="pageSize">Authorized user</param>
        /// <param name="sortKey">Key for sorting</param>
        /// <param name="auth">Whether to select reports of that user is author</param>
        /// <returns>List of ClubAnnualReportTableObjectl</returns>
        Task<IEnumerable<ClubAnnualReportTableObject>> GetAllAsync(User user, bool isAdmin, string searchedData, int page, int pageSize, int sortKey, bool auth);

        /// <summary>
        /// Method to check whether club has created annual report
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <param name="clubId">Club identification number</param>
        /// <returns>Information whether club has created annual report</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when user hasn't access to club</exception>
        /// <exception cref="System.NullReferenceException">Thrown when club doesn't exist</exception>
        Task<bool> CheckCreated(User user, int clubId);

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
        Task<IEnumerable<ClubReportAdministrationDTO>> GetClubReportAdminsAsync(int ClubAnnualReportID);
        Task<IEnumerable<ClubMemberHistoryDTO>> GetClubReportMembersAsync(int ClubAnnualReportID);
    }
}
