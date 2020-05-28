using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AutoMapper;
using System.Linq;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ClubController(IClubService clubService, IMapper mapper, UserManager<User> userManager)
        {
            _clubService = clubService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var clubs = _clubService.GetAllClubs();
            var viewModels = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs);

            return View(viewModels);
        }

        public IActionResult Club(int index)
        {
            try
            {
                var viewModel= _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubProfile(index));
                ViewBag.usermanager = _userManager;
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
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetCurrentClubAdminByID(index));
                ViewBag.usermanager = _userManager;
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubMembers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubMembersOrFollowers(index, true));
                ViewBag.usermanager = _userManager;
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubFollowers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubMembersOrFollowers(index, false));
                ViewBag.usermanager = _userManager;
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        
        public IActionResult ClubDescription(int index)
        {
            try
            {
                var viewModel=_mapper.Map<ClubDTO, ClubViewModel>(_clubService.GetById(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult EditClub(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubDTO, ClubViewModel>(_clubService.GetById(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult EditClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                _clubService.Update(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = model.ID });
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
                _clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
                _clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
                _clubService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
            bool isSuccessfull = _clubService.DeleteClubAdmin(adminId);

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
                _clubService.SetAdminEndDate(adminEndDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [HttpPost]
        public IActionResult AddToClubAdministration([FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                _clubService.AddClubAdmin(createdAdmin);

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        
        public IActionResult ChooseAClub()
        {
            var clubs = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(_clubService.GetAllClubs());

            ViewBag.usermanager = _userManager;

            return View(clubs);
        }
        
        public IActionResult AddAsClubFollower(int clubIndex)
        {
            var userId = _userManager.GetUserId(User);

            _clubService.AddFollower(clubIndex, userId);

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
                var club = _clubService.Create(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}