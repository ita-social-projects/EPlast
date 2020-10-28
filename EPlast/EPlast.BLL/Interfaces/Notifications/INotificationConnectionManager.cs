using EPlast.BLL.DTO.Notification;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Notifications
{
    public interface INotificationConnectionManager
    {
        WebSocket GetSocketByConnectionId(string id);

        HashSet<ConnectionDTO> GetConnectionsByUserId(string userId);

        ConcurrentDictionary<string, HashSet<ConnectionDTO>> GetAll();

        string GetUserId(WebSocket socket);

        string GetConnectionId(WebSocket socket);

        string AddSocket(string userId, WebSocket socket);

        Task<bool> RemoveSocket(string userId, string connectionId);

        void RemoveAllUserIdSockets(string userId);

        Task SendMessageAsync(string userId, string message);

        IEnumerable<string> OnlineUsers { get; }
    }
}
