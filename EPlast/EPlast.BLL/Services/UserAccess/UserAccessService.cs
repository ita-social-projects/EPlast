using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserAccess
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IClubAccessService _clubAccessService;
        private readonly ISecurityModel _securityModel;

        private const string ClubSecuritySettingsFile = "ClubAccessSettings.json";

        public UserAccessService(IClubAccessService clubAccessService, ISecurityModel securityModel)
        {
            _clubAccessService = clubAccessService;
            _securityModel = securityModel;
        }

        public async Task<Dictionary<string, bool>> GetUserClubAccessAsync(int clubId, string userId, User user)
        {
            _securityModel.SetSettingsFile(ClubSecuritySettingsFile);
            var userAccess = await _securityModel.GetUserAccessAsync(userId);
            userAccess["EditClub"] = await _clubAccessService.HasAccessAsync(user, clubId);
            return userAccess;
        }
    }
}