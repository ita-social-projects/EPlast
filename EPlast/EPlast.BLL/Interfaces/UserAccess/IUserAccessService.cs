using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Returns dictionary with user accesses for annual reports
        /// cityReportId is optional if we want to check user access for certain city report
        /// </summary>
        Task<Dictionary<string, bool>> GetUserAnnualReportAccessAsync(string userId, int? cityReportId = null);

        /// <summary>
        /// Returns dictionary with user accesses for Statistics
        /// </summary>
        Task<Dictionary<string, bool>> GetUserStatisticsAccessAsync(string userId);

        /// <summary>
        /// Returns dictionary with user accesses for events
        /// </summary>
        Task<Dictionary<string, bool>> GetUserEventAccessAsync(string userId, User user, int? eventId = null);
    }
}
