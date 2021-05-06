using EPlast.BLL.Interfaces.Notifications;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EPlast.WebApi.WebSocketHandlers
{

    public abstract class BaseWebSocketHandler
    {
        protected INotificationConnectionManager WebSocketConnectionManager { get; set; }

        public BaseWebSocketHandler(INotificationConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual string OnConnected(string userId, WebSocket socket)
        {
            return WebSocketConnectionManager.AddSocket(userId, socket);
        }

        public virtual async Task OnDisconnectedAsync(string userId, string connectionId)
        {
            await WebSocketConnectionManager.RemoveSocketAsync(userId, connectionId);
        }

        public async Task SendMessageAsync(string userId, string message)
        {
            await WebSocketConnectionManager.SendMessageAsync(userId, message);
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
