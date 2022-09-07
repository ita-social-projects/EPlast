using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EPlast.WebApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
    }
    public interface INotificationHub
    {
        public Task Send(string text);
    }
}