using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Notifications
{
    public class NotificationConnectionManager : INotificationConnectionManager
    {

        private readonly IUniqueIdService _uniqueId;
        private readonly IUserMapService _userMap;

        public NotificationConnectionManager(IUniqueIdService uniqueId, IUserMapService UserMap)
        {
            _uniqueId = uniqueId;
            _userMap = UserMap;
        }

        public IEnumerable<string> OnlineUsers { get { return _userMap.UserConnections.Keys; } }

        public WebSocket GetSocketByConnectionId(string id)
        {
            foreach (var item in _userMap.UserConnections)
            {
                var connection = item.Value.FirstOrDefault(conn => conn.ConnectionId == id);
                if (connection != null)
                {
                    return connection.WebSocket;
                }
            }
            throw new InvalidOperationException();
        }

        public string GetUserId(WebSocket socket)
        {
            var userConnections = _userMap.UserConnections.FirstOrDefault(p => p.Value.FirstOrDefault(conn => conn.WebSocket == socket) != null);
            if (userConnections.Equals(default(KeyValuePair<string, HashSet<ConnectionDTO>>)))
            {
                throw new ArgumentException("Dictionary doesn`t contain this WebSocket", "socket");
            }
            return userConnections.Key;
        }

        public string AddSocket(string userId, WebSocket socket)
        {
            var connectionId = _uniqueId.GetUniqueId().ToString();
            if (!_userMap.UserConnections.ContainsKey(userId))
            {
                _userMap.UserConnections.TryAdd(userId, new HashSet<ConnectionDTO>());
            }
            _userMap.UserConnections[userId].Add(new ConnectionDTO { ConnectionId = connectionId, WebSocket = socket });
            return connectionId;
        }

        public async Task<bool> RemoveSocketAsync(string userId, string connectionId)
        {
            WebSocket socket = GetSocketByConnectionId(connectionId);
            var removedSocket = _userMap.UserConnections[userId].FirstOrDefault(e => e.ConnectionId == connectionId);
            _userMap.UserConnections[userId].Remove(removedSocket);
            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
            return removedSocket != null;
        }

        public async Task SendMessageAsync(string userId, string message)
        {
            if (_userMap.UserConnections.ContainsKey(userId))
            {
                var tasks = _userMap.UserConnections[userId].Where(c => c.WebSocket.State == WebSocketState.Open)
                .Select(c =>
                    {
                        var byteArray = Encoding.UTF8.GetBytes(message);
                        return c.WebSocket.SendAsync(buffer: new ArraySegment<byte>(array: byteArray,
                                                                                    offset: 0,
                                                                                    count: byteArray.Length),
                                                    messageType: WebSocketMessageType.Text,
                                                    endOfMessage: true,
                                                    cancellationToken: CancellationToken.None);
                    });
                await Task.WhenAll(tasks);
            }
        }
    }
}

