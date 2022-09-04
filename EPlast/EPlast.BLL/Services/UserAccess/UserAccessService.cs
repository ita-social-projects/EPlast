using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IClubAccessService _clubAccessService;
        private readonly IEventUserAccessService _eventAccessService;
        private readonly ICityAccessService _cityAccessService;
        private readonly IRegionAccessService _regionAccessService;
        private readonly IAnnualReportAccessService _annualReportAccessService;
        private readonly IUserProfileAccessService _userProfileAccessService;
        private readonly ISecurityModel _securityModel;

        private const string ClubSecuritySettingsFile = "ClubAccessSettings.json";
        private const string DistinctionSecuritySettingsFile = "DistinctionsAccessSettings.json";
        private const string EventUserSecuritySettingsFile = "EventUserAccessSettings.json";
        private const string CitySecuritySettingsFile = "CityAccessSettings.json";
        private const string RegionSecuritySettingsFile = "RegionAccessSettings.json";
        private const string AnnualReportSecuritySettingsFile = "AnnualReportAccessSettings.json";
        private const string StatisticsSecuritySettingsFile = "StatisticsAccessSettings.json";
        private const string UserProfileAccessSettings = "UserProfileAccessSettings.json";
        private const string MenuAccessSettingsFile = "MenuAccessSettings.json";
        private const string PrecautionsAccessSettingsFile = "PrecautionsAccessSettings.json";

        public UserAccessService(IUserAccessWrapper userAccessWrapper, ISecurityModel securityModel)
        {
            _clubAccessService = userAccessWrapper.ClubAccessService;
            _eventAccessService = userAccessWrapper.EventAccessService;
            _cityAccessService = userAccessWrapper.CityAccessService;
            _regionAccessService = userAccessWrapper.RegionAccessService;
            _annualReportAccessService = userAccessWrapper.AnnualReportAccessService;
            _userProfileAccessService = userAccessWrapper.UserProfileAccessService;
            _securityModel = securityModel;
        }

        public async Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId, string userId, User user)
        {
            _securityModel.SetSettingsFile(ClubSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["EditClub"] = await _clubAccessService.HasAccessAsync(user, clubId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserDistinctionAccessAsync(string userId)
        {
            _securityModel.SetSettingsFile(DistinctionSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserEventAccessAsync(string userId, User user, int? eventId = null)
        {
            _securityModel.SetSettingsFile(EventUserSecuritySettingsFile);
            var defaultUserAccesses = await _securityModel.GetUserAccessAsync(userId);
            var userAccesses = await _eventAccessService.RedefineAccessesAsync(defaultUserAccesses, user, eventId);
            return userAccesses;
        }

        public async Task<Dictionary<string, bool>> GetUserCityAccessAsync(int cityId, string userId, User user)
        {
            _securityModel.SetSettingsFile(CitySecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["EditCity"] = await _cityAccessService.HasAccessAsync(user, cityId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserRegionAccessAsync(int regionId, string userId, User user)
        {
            _securityModel.SetSettingsFile(RegionSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["EditRegion"] = await _regionAccessService.HasAccessAsync(user, regionId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserAnnualReportAccessAsync(string userId, User user, ReportType? reportType=null, int? reportId=null)
        {
            _securityModel.SetSettingsFile(AnnualReportSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["CanViewReportDetails"] = await _annualReportAccessService.CanViewReportDetailsAsync(user, userAccess["CanViewReportDetails"], reportType, reportId);
            userAccess["CanEditReport"] = await _annualReportAccessService.CanEditReportAsync(user, userAccess["CanEditReport"], reportType, reportId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserProfileAccessAsync(string userId, string focusUserId, User user)
        {
            _securityModel.SetSettingsFile(UserProfileAccessSettings);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["CanViewUserFullProfile"] = await _userProfileAccessService.CanViewFullProfile(user, focusUserId);
            userAccess["CanApproveAsClubHead"] = await _userProfileAccessService.CanApproveAsHead(user, focusUserId, Roles.KurinHead);
            userAccess["CanApproveAsCityHead"] = await _userProfileAccessService.CanApproveAsHead(user, focusUserId, Roles.CityHead);
            userAccess["CanEditUserProfile"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanEditDeleteUserPhoto"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanSeeAddDeleteUserExtractUPU"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanAddUserDistionction"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanDeleteUserDistinction"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanDownloadUserDistinction"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            userAccess["CanViewDownloadUserBiography"] = await _userProfileAccessService.CanEditUserProfile(user, focusUserId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserStatisticsAccessAsync(string userId)
        {
            _securityModel.SetSettingsFile(StatisticsSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserMenuAccessAsync(string userId)
        {
            _securityModel.SetSettingsFile(MenuAccessSettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            return userAccess;
        }

        public async Task<Dictionary<string, bool>> GetUserPrecautionsAccessAsync(string userId)
        {
            _securityModel.SetSettingsFile(PrecautionsAccessSettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            return userAccess;
        }
    }
}
