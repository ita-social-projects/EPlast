using System.Collections.Concurrent;
using System.Collections.Generic;
using EPlast.BLL.DTO.Notification;

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
        ConcurrentDictionary<string, HashSet<ConnectionDto>> UserConnections { get; }
    }
}
