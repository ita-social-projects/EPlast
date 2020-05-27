using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService clubService;
        private readonly IClubMembersService clubMembersService;
        private readonly IClubAdministrationService clubAdministrationService;
        private readonly IMapper mapper;
        private readonly IUserManagerService userManagerService;

        public ClubController(IMapper mapper, IUserManagerService userManagerService, IClubService clubService, IClubMembersService clubMembersService, IClubAdministrationService clubAdministrationService)
        {
            this.mapper = mapper;
            this.userManagerService = userManagerService;
            this.clubService = clubService;
            this.clubMembersService = clubMembersService;

            this.clubAdministrationService = clubAdministrationService;
        }

        public IActionResult Index()
        {
            ViewBag.usermanager = userManagerService;

            var clubs = clubService.GetAllDTO();
            return View(mapper.Map<IEnumerable<ClubDTO>, IEnumerable<CLubViewModel> >(clubs));
        }

        public IActionResult Club(int index)
        {
            try
            {
                var clubDTO = clubService.GetByIdWithDetailsDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);
                var members = mapper.Map< IEnumerable<ClubMembersDTO>,
                    IEnumerable<ClubMembersViewModel>> (clubMembersService.GetClubMembersDTO(clubDTO, true, 6)).ToList();
                var followers = mapper.Map<IEnumerable<ClubMembersDTO>,
                    IEnumerable<ClubMembersViewModel>>(clubMembersService.GetClubMembersDTO(clubDTO, false, 6)).ToList();
                var clubAdmin = mapper.Map < UserDTO, UserViewModel> 
                    (clubAdministrationService.GetCurrentClubAdmin(clubDTO));

                ViewBag.usermanager = userManagerService;

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
                var clubDTO = clubService.GetByIdWithDetailsDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);
                var clubAdmin = mapper.Map < UserDTO, UserViewModel> 
                    (clubAdministrationService.GetCurrentClubAdmin(clubDTO));

                ViewBag.usermanager = userManagerService;

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
                var clubDTO = clubService.GetByIdWithDetailsDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);
                var members = mapper.Map<IEnumerable<ClubMembersDTO>,
                    IEnumerable<ClubMembersViewModel>>(clubMembersService.GetClubMembersDTO(clubDTO, true)).ToList();
                var clubAdmin = mapper.Map<UserDTO, UserViewModel>
                    (clubAdministrationService.GetCurrentClubAdmin(clubDTO));

                ViewBag.usermanager = userManagerService;

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
                var clubDTO = clubService.GetByIdWithDetailsDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);
                
                var followers = mapper.Map<IEnumerable<ClubMembersDTO>,
                    IEnumerable<ClubMembersViewModel>>(clubMembersService.GetClubMembersDTO(clubDTO, false)).ToList();
                var clubAdmin = mapper.Map<UserDTO, UserViewModel>
                    (clubAdministrationService.GetCurrentClubAdmin(clubDTO));

                ViewBag.usermanager = userManagerService;

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
                var clubDTO = clubService.GetByIdDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);

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
                var clubDTO = clubService.GetByIdDTO(index);
                var club = mapper.Map<ClubDTO, CLubViewModel>(clubDTO);

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
                clubService.Update(mapper.Map <CLubViewModel, ClubDTO> (model.Club), file);

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
                clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
                clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
                clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

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
            bool isSuccessfull = clubAdministrationService.DeleteClubAdmin(adminId);

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
                clubAdministrationService.SetAdminEndDate(adminEndDate);

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
                clubAdministrationService.AddClubAdmin(createdAdmin);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public IActionResult ChooseAClub()
        {
            var clubs = clubService.GetAllDTO();
            ViewBag.usermanager = userManagerService;
            return View(mapper.Map<IEnumerable<ClubDTO>, IEnumerable<CLubViewModel>>(clubs));
        }
        public IActionResult AddAsClubFollower(int clubIndex)
        {
            var userId = userManagerService.GetUserId(User);

            clubMembersService.AddFollower(clubIndex, userId);

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
                clubService.Create(mapper.Map < CLubViewModel, ClubDTO > (club), file);

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}
