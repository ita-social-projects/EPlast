using EPlast.BLL.Interfaces.Notifications;
using System.Collections.Generic;

namespace EPlast.BLL.Services.Notifications
{
    public class ConnectionManagerService : IConnectionManagerService
    {
        private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();

        public IEnumerable<string> OnlineUsers { get { return userMap.Keys; } }

        public void AddConnection(string userId, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(userId))
                {
                    userMap[userId] = new HashSet<string>();
                }
                userMap[userId].Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMap)
            {
                foreach (var userId in userMap.Keys)
                {
                    if (userMap[userId].Contains(connectionId))
                    {
                        userMap[userId].Remove(connectionId);
                        break;
                    }
                }
            }
        }

        public HashSet<string> GetConnections(string userId)
        {
            HashSet<string> connections;
            lock (userMap)
            {
                if (userMap.ContainsKey(userId))
                {
                    connections = userMap[userId];
                }
                else
                {
                    connections = null;
                }
            }
            return connections;
        }

    }
}
