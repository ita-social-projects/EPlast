using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Interfaces.Notifications
{
    public interface IConnectionManagerService
    {
        void AddConnection(string userId, string connectionId);

        void RemoveConnection(string connectionId);

        HashSet<string> GetConnections(string userId);

        IEnumerable<string> OnlineUsers { get; }
    }
}
