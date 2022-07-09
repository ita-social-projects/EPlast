using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace EPlast.BLL.DTO.Notification
{
    public class ConnectionDto
    {
        public string ConnectionId { get; set; }
        public WebSocket WebSocket { get; set; }
    }
}
