using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService clubService;
        private readonly UserManager<User> _userManager;

        public ClubController(IClubService clubService, UserManager<User> userManager)
        {
            this.clubService = clubService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var clubs = clubService.GetAllClubs()
                .Select(club => new ClubViewModel { Club = club })
                .ToList();

            ViewBag.usermanager = _userManager;

            return View(clubs);
        }

        public IActionResult Club(int index)
        {
            try
            {
                var club = clubService.GetByIdWithDetails(index);
                var members = clubService.GetClubMembers(club, true, 6);
                var followers = clubService.GetClubMembers(club, false, 6);
                var clubAdmin = clubService.GetCurrentClubAdmin(club);

                ViewBag.usermanager = _userManager;

                var viewModel = new ClubViewModel
                {
                    Club = club,
                    ClubAdmin = clubAdmin,
                    Members = members,
                    Followers = followers
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubAdmins(int index)
        {
            try
            {
                var club = clubService.GetByIdWithDetails(index);
                var clubAdmin = clubService.GetCurrentClubAdmin(club);

                ViewBag.usermanager = _userManager;

                var viewModel = new ClubViewModel
                {
                    Club = club,
                    ClubAdmin = clubAdmin,
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubMembers(int index)
        {
            try
            {
                var club = clubService.GetByIdWithDetails(index);
                var members = clubService.GetClubMembers(club, true);
                var clubAdmin = clubService.GetCurrentClubAdmin(club);

                ViewBag.usermanager = _userManager;

                var viewModel = new ClubViewModel
                {
                    Club = club,
                    ClubAdmin = clubAdmin,
                    Members = members
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubFollowers(int index)
        {
            try
            {
                var club = clubService.GetByIdWithDetails(index);
                var followers = clubService.GetClubMembers(club, false);
                var clubAdmin = clubService.GetCurrentClubAdmin(club);

                ViewBag.usermanager = _userManager;

                var viewModel = new ClubViewModel {
                    Club = club,
                    ClubAdmin = clubAdmin,
                    Followers = followers
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubDescription(int index)
        {
            try
            {
                var club = clubService.GetById(index);

                return View(new ClubViewModel { Club = club });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult EditClub(int index)
        {
            try
            {
                var club = clubService.GetById(index);

                return View(new ClubViewModel { Club = club });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult EditClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                clubService.Update(model.Club, file);

                return RedirectToAction("Club", new { index = model.Club.ID });
            }
            catch (Exception)
            {

                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubMembers", new { index = clubIndex });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusFollowers(int index, int clubIndex)
        {
            try
            {
                clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubFollowers", new { index = clubIndex });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("Club", new { index = clubIndex });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessfull = clubService.DeleteClubAdmin(adminId);

            if (isSuccessfull)
            {
                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }
            else
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public int AddEndDate([FromBody] AdminEndDateDTO adminEndDate)
        {
            try
            {
                clubService.SetAdminEndDate(adminEndDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [HttpPost]
        public int AddToClubAdministration([FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                clubService.AddClubAdmin(createdAdmin);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public IActionResult ChooseAClub()
        {
            var clubs = clubService.GetAllClubs()
                .Select(club => new ClubViewModel { Club = club })
                .ToList();

            ViewBag.usermanager = _userManager;

            return View(clubs);
        }
        public IActionResult AddAsClubFollower(int clubIndex)
        {
            var userId = _userManager.GetUserId(User);

            clubService.AddFollower(clubIndex, userId);

            return RedirectToAction("UserProfile", "Account", new { userId = userId });
        }
        [HttpGet]
        public IActionResult CreateClub()
        {
            try
            {
                return View(new ClubViewModel());
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpPost]
        public IActionResult CreateClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                var club = model.Club;
                clubService.Create(club, file);

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
