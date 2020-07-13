using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        private readonly IStringLocalizer<AuthenticationErrors> _resourceForErrors;

        public HomeController(IHomeService homeService, 
            IStringLocalizer<AuthenticationErrors> resourceForErrors)
        {
            _homeService = homeService;
            _resourceForErrors = resourceForErrors;
        }

        [HttpPost("sendFeedback")]
        public async Task<IActionResult> SendContacts([FromBody]ContactsDto contactsDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return BadRequest();
            }
            await _homeService.SendEmailAdmin(contactsDto);

            return Ok(_resourceForErrors["Feedback-Sended"]);
        }
    }
}