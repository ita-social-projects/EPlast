using EPlast.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AAAAAController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

        public AAAAAController(IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Send()
        {
            await _hubContext.Clients.All.Send("Саксесс");
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Send(string id)
        {
            await _hubContext.Clients.User(id).Send("Саксесс");
            return NoContent();
        }
    }
}
