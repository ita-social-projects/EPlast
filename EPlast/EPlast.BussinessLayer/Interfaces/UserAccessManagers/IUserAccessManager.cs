namespace EPlast.BussinessLayer.AccessManagers.Interfaces
{
    public interface IUserAccessManager
    {
        bool HasAccess(string userId, string userTargetId);
    }
}