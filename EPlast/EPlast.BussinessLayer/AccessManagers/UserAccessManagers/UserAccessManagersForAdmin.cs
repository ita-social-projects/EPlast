using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.UserAccessManagers
{
    public class UserAccessManagersForAdmin : IUserAccessManager
    {
        public bool HasAccess(string userId, string userTargetId)
        {
            return true;
        }
    }
}