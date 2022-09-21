using EPlast.DataAccess.Entities;
using EPlast.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;

namespace EPlast.BLL.Interfaces.UserAccess
{
    public interface IUserAccessService
    {
        /// <summary>
        /// Returns dictionary with user accesses for clubs
        /// </summary>
        Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId, string userId, User user);

        /// <summary>
        /// Returns dictionary with user accesses for distinctions
        /// </summary>
        Task<Dictionary<string, bool>> GetUserDistinctionAccessAsync(string userId);

        /// <summary>
        /// Returns dictionary with user accesses for cities
        /// </summary>
        Task<Dictionary<string, bool>> GetUserCityAccessAsync(int cityId, string userId, User user);

        /// <summary>
        /// Returns dictionary with user accesses for regions
        /// </summary>
        Task<Dictionary<string, bool>> GetUserRegionAccessAsync(int regionId, string userId, User user);

        Task<Dictionary<string, bool>> GetUserRegionAdministrationAccessAsync( RegionAdministrationDto regionAdministration, User user);

        /// <summary>
        /// Returns dictionary with user accesses for annual reports
        /// report type and report id is optional (check access to edit certain report)
        /// report type
        /// 0 - city
        /// 1 - club
        /// 2 - region
        /// </summary>
        Task<Dictionary<string, bool>> GetUserAnnualReportAccessAsync(string userId, User user, ReportType? reportType = null, int? reportId = null);

        /// <summary>
        /// Returns dictionary with user accesses for statistics
        /// </summary>
        Task<Dictionary<string, bool>> GetUserStatisticsAccessAsync(string userId);

        /// <summary>
        /// Returns dictionary with user accesses for events
        /// </summary>
        Task<Dictionary<string, bool>> GetUserEventAccessAsync(string userId, User user, int? eventId = null);

        /// <summary>
        /// Returns dictionary with user accesses for UserProfile
        /// </summary>
        Task<Dictionary<string, bool>> GetUserProfileAccessAsync(string userId, string focusUserId, User user);

        /// <summary>
        /// Returns dictionary with user accesses for menu
        /// </summary>
        Task<Dictionary<string, bool>> GetUserMenuAccessAsync(string userId);

        Task<Dictionary<string, bool>> GetUserPrecautionsAccessAsync(string userId);

    }
}