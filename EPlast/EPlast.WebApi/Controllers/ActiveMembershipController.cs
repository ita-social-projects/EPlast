using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMembershipController : ControllerBase
    {
        private IPlastDegreeService _activeMembershipService;
        public ActiveMembershipController(IPlastDegreeService activeMembershipService)
        {
            _activeMembershipService = activeMembershipService;
        }
        [HttpGet("dergees")]
        public async Task<IEnumerable<PlastDegreeDTO>> GetAllDergees()
        {
            return await _activeMembershipService.GetDergeesAsync();
        }
       

    }
}