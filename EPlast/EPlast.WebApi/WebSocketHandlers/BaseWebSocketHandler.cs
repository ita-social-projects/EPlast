using EPlast.BLL.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual async Task OnDisconnected(string userId, string connectionId)
        {
            await WebSocketConnectionManager.RemoveSocket(userId, connectionId);
        }

        public async Task SendMessageAsync(string userId, string message)
        {
            await WebSocketConnectionManager.SendMessageAsync(userId, message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            List<Task> tasks = new List<Task>();
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                tasks.AddRange(pair.Value.Where(c => c.WebSocket.State == WebSocketState.Open)
                                         .Select(c => SendMessageAsync(pair.Key, message)));
                
            }
            await Task.WhenAll(tasks);
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
