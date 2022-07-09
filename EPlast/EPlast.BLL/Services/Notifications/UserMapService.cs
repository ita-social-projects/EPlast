using System.Collections.Concurrent;
using System.Collections.Generic;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;

namespace EPlast.BLL.Services.Notifications
{
    public class UserMapService : IUserMapService
    {
        private static readonly ConcurrentDictionary<string, HashSet<ConnectionDto>> userMap = new ConcurrentDictionary<string, HashSet<ConnectionDto>>();
        public ConcurrentDictionary<string, HashSet<ConnectionDto>> UserConnections { get { return userMap; } }
    }
}
