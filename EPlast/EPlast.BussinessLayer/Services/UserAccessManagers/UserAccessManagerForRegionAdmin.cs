using System.Linq;
using Microsoft.EntityFrameworkCore;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.UserAccessManagers
{
    public class UserAccessManagerForRegionAdmin : IUserAccessManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public UserAccessManagerForRegionAdmin(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public bool HasAccess(string userId, string userTargetId)
        {
            try
            {
                var regionAdministration = _repoWrapper.RegionAdministration
                    .FindByCondition(ra => ra.User.Id == userId && ra.EndDate == null)
                    .Include(ra => ra.Region)
                    .First();
                var citiMembers = _repoWrapper.CityMembers
                    .FindByCondition(cm => cm.UserId == userTargetId && cm.EndDate == null &&
                        cm.City.Region.ID == regionAdministration.Region.ID)
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