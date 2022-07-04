using System.Net.WebSockets;

namespace EPlast.BLL.DTO.Notification
{
    public class ConnectionDto
    {
        public string ConnectionId { get; set; }
        public WebSocket WebSocket { get; set; }
    }
}
