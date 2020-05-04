using System.Linq;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.UserAccessManagers
{
    public class UserAccessManagerForCityAdmin : IUserAccessManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public UserAccessManagerForCityAdmin(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public bool HasAccess(string userId, string userTargetId)
        {
            try
            {
                var cityAdministration = _repoWrapper.CityAdministration.
                    FindByCondition(ca => ca.UserId == userId && ca.EndDate == null)
                    .First();
                var citiMembers = _repoWrapper.CityMembers
                    .FindByCondition(cm => cm.UserId == userTargetId && cm.EndDate == null
                        && cm.CityId == cityAdministration.CityId)
                    .FirstOrDefault();
                if (citiMembers != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}