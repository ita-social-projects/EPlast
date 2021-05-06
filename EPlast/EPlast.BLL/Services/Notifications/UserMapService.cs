using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EPlast.BLL.Services.Notifications
{
    public class UserMapService : IUserMapService
    {
        private static readonly ConcurrentDictionary<string, HashSet<ConnectionDTO>> userMap = new ConcurrentDictionary<string, HashSet<ConnectionDTO>>();
        public ConcurrentDictionary<string, HashSet<ConnectionDTO>> UserConnections { get { return userMap; } }
    }
}
