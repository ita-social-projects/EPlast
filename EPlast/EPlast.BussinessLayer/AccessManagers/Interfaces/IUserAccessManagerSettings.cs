using System.Collections.Generic;

namespace EPlast.BussinessLayer.AccessManagers.Interfaces
{
    public interface IUserAccessManagerSettings
    {
        Dictionary<string, IUserAccessManager> GetUserAccessManagers();
    }
}