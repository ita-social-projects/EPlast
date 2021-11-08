using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IClubAccessService _clubAccessService;
        private readonly ICityAccessService _cityAccessService;
        private readonly ISecurityModel _securityModel;

        private const string ClubSecuritySettingsFile = "ClubAccessSettings.json";
        private const string CitySecuritySettingsFile = "CityAccessSettings.json";

        public UserAccessService(IClubAccessService clubAccessService, ICityAccessService cityAccessService, ISecurityModel securityModel)
        {
            _clubAccessService = clubAccessService;
            _cityAccessService = cityAccessService;
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
    }
}