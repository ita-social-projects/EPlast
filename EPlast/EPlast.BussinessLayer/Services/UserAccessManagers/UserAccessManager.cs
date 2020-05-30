using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers
{
    public class UserAccessManager : IUserAccessManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        private readonly Dictionary<string, IUserAccessManager> userAccessManagers;

        public UserAccessManager(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager, IUserAccessManagerSettings managerSettings)
        {
            _repoWrapper = repositoryWrapper;
            _userManager = userManager;
            userAccessManagers = managerSettings.GetUserAccessManagers();
        }

        public bool HasAccess(string userId, string userTargetId)
        {
            var roles = GetRoles(userId);
            foreach (var role in roles)
            {
                if (userAccessManagers.ContainsKey(role))
                {
                    return userAccessManagers[role].HasAccess(userId, userTargetId);
                }
            }
            return false;
        }

        private IEnumerable<string> GetRoles(string userId)
        {
            try
            {
                var user = _repoWrapper.User
                    .FindByCondition(u => u.Id == userId)
                    .First();
                var roles = _userManager.GetRolesAsync(user);
                return roles.Result;
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}