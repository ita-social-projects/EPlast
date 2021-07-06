using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Notifications
{
    public interface INotificationConnectionManager
    {
        WebSocket GetSocketByConnectionId(string id);
        string GetUserId(WebSocket socket);
        string AddSocket(string userId, WebSocket socket);
        Task<bool> RemoveSocketAsync(string userId, string connectionId);
        Task SendMessageAsync(string userId, string message);
        IEnumerable<string> OnlineUsers { get; }
    }
}
