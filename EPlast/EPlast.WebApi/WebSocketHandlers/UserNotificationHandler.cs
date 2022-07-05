using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EPlast.WebApi.WebSocketHandlers
{
    public class UserNotificationHandler : BaseWebSocketHandler
    {

        public UserNotificationHandler(INotificationConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager) { }

        public async Task SendUserNotificationAsync(UserNotificationDto userNotification)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            await SendMessageAsync(userNotification.OwnerUserId, JsonConvert.SerializeObject(userNotification, 
                new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                })
            );
        }

        public IEnumerable<string> GetOnlineUsers()
        {
            return WebSocketConnectionManager.OnlineUsers;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string userId;
            try
            {
                userId = WebSocketConnectionManager.GetUserId(socket);
            }
            catch (Exception)
            {
                return;
            }
            var message = $"{userId} send: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageAsync(userId, message);
        }
    }
}
