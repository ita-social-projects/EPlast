using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Services.Interfaces;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IClubAccessService _clubAccessService;
        private readonly ICityAccessService _cityAccessService;
        private readonly IRegionAccessService _regionAccessService;
        private readonly IAnnualReportAccessService _annualReportAccessService;
        private readonly ISecurityModel _securityModel;

        private const string ClubSecuritySettingsFile = "ClubAccessSettings.json";
        private const string CitySecuritySettingsFile = "CityAccessSettings.json";
        private const string RegionSecuritySettingsFile = "RegionAccessSettings.json";
        private const string AnnualReportSecuritySettingsFile = "AnnualReportAccessSettings.json";

        public UserAccessService(IClubAccessService clubAccessService, ICityAccessService cityAccessService, IRegionAccessService regionAccessService,IAnnualReportAccessService annualReportAccessService, ISecurityModel securityModel)
        {
            _clubAccessService = clubAccessService;
            _cityAccessService = cityAccessService;
            _regionAccessService = regionAccessService;
            _annualReportAccessService = annualReportAccessService;
            _securityModel = securityModel;
        }

        public async Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId, string userId, User user)
        {
            _securityModel.SetSettingsFile(ClubSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["EditClub"] = await _clubAccessService.HasAccessAsync(user, clubId);
            return userAccess;
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

        public async Task<Dictionary<string, bool>> GetUserAnnualReportAccessAsync(string userId, int? cityReportId=null)
        {
            _securityModel.SetSettingsFile(AnnualReportSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            if (cityReportId != null)
            {
                userAccess["EditReport"] = await _annualReportAccessService.CanEditCityReportAsync(userId, (int)cityReportId);
            }
            return userAccess;
        }
    }
}
