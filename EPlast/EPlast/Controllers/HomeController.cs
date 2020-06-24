using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Repositories;
using EPlast.Models;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeService homeService, IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _homeService = homeService;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult AboutPLAST()
        {
            return View("AboutPLAST");
        }

        public IActionResult Contacts()
        {
            return View("Views/Home/Contacts.cshtml");
        }

        public IActionResult FAQ()
        {
            return View("FAQ");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("User/GetPage")]
        public IActionResult GetInformation()
        {
            return View("Views/Account/Login.cshtml");
        }


        public IActionResult Search(string search)
        {
            var surnames = _repoWrapper.User.FindByCondition(q => q.LastName.StartsWith(search));
            var names = _repoWrapper.User.FindByCondition(q => q.FirstName.StartsWith(search));
            var model = new SearchSurname();
            model.Users = surnames;
            return View(model);
        }

        [HttpPost]
        public IActionResult GetSearchUser(string userId)
        {
            var res = _repoWrapper.User.FindByCondition(x => x.Id == userId);
            return PartialView(res);
        }

        [HttpGet]
        public IActionResult FeedBackSended()
        {
            return View("FeedBackSended");
        }

        [HttpPost]
        public async Task<IActionResult> SendContacts(ContactsViewModel contactsViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return View("Contacts");
            }

            var contact = _mapper.Map<ContactDTO>(contactsViewModel);

            await _homeService.SendEmailAdmin(contact);

            return RedirectToAction("FeedBackSended", "Home");
        }
    }
}