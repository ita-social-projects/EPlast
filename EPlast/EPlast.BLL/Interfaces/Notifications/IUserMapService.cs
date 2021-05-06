using EPlast.BLL.DTO.Notification;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EPlast.BLL.Interfaces.Notifications
{
    /// <summary>
    /// Stores the user connection data
    /// </summary>
    public interface IUserMapService
    {
        /// <summary>
        /// Returns the list of user connections
        /// </summary>
        ConcurrentDictionary<string, HashSet<ConnectionDTO>> UserConnections { get; }
    }
}
