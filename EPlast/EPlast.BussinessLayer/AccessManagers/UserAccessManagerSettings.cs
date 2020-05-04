using System.Collections.Generic;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.AccessManagers.UserAccessManagers;

namespace EPlast.BussinessLayer.AccessManagers
{
    public class UserAccessManagerSettings : IUserAccessManagerSettings
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public UserAccessManagerSettings(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public Dictionary<string, IUserAccessManager> GetUserAccessManagers()
        {
            var userAccessManagers = new Dictionary<string, IUserAccessManager>
            {
                { "Admin", new UserAccessManagersForAdmin() },
                { "Голова Округу", new UserAccessManagerForRegionAdmin(_repoWrapper) },
                { "Голова Станиці", new UserAccessManagerForCityAdmin(_repoWrapper) }
            };
            return userAccessManagers;
        }
    }
}