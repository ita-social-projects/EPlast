using System;
using EPlast.BLL.Interfaces.RegionBoard;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;

namespace EPlast.BLL.Services
{
    public class RegionsBoardService : IRegionsBoardService
    {
        private readonly ISecurityModel _securityModel;
        private const string SecuritySettingsFile = "RegionsBoardAccessSettings.json";

        public RegionsBoardService(ISecurityModel securityModel)
        {
            _securityModel = securityModel;
            _securityModel.SetSettingsFile(SecuritySettingsFile);
        }
        public async Task<Dictionary<string, bool>> GetUserAccess(string userId)
        {
            var userAcesses = _securityModel.GetUserAccess(userId);
            return userAcesses;
        }
    }
}