using EPlast.BLL.Interfaces.Notifications;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.SignalRHubs
{
    public class NotificationHub : Hub
    {
        private IConnectionManagerService _connectionManagerService;

        public NotificationHub(IConnectionManagerService connectionManagerService)
        {
            _connectionManagerService = connectionManagerService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            _connectionManagerService.AddConnection(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectionManagerService.RemoveConnection(Context.ConnectionId); 
            await base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
