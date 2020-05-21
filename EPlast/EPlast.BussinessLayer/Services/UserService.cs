using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BussinessLayer.Services
{
    public class UserService:IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;
        public UserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
            _env = env;
        }

        public void Delete(string user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserProfile(string userId)
        {
            var user = _repoWrapper.User.
                FindByCondition(q => q.Id == userId).
                Include(i => i.UserProfile).
                    ThenInclude(x => x.Nationality).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Gender).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Education).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Degree).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Religion).
                Include(g => g.UserProfile).
                    ThenInclude(g => g.Work).
                Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                        ThenInclude(q => q.User).
                    FirstOrDefault();
            var model = _mapper.Map<User, UserDTO>(user);
            return model;
        }

        public async Task<IEnumerable<ConfirmedUserDTO>> GetConfirmedUsers(UserDTO user)
        {
            var result = user.ConfirmedUsers.Where(x => x.isCityAdmin == false && x.isClubAdmin == false);
            return result;
        }

        public async Task<ConfirmedUserDTO> GetClubAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.FirstOrDefault(x => x.isClubAdmin == true);
            return result;
        }

        public async Task<ConfirmedUserDTO> GetCityAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.FirstOrDefault(x => x.isCityAdmin == true);
            return result;
        }
        public async Task<bool> CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, ClaimsPrincipal user)
        {
            var currentUserId = _userManager.GetUserId(user);

            var canApprove = confUsers.Count() < 3
                    && !confUsers.Any(x => x.Approver.UserID == currentUserId)
                    && !(currentUserId == userId);
            return canApprove;
        }
        public async Task<TimeSpan> CheckOrAddPlastunRole(string userId,DateTime registeredOn)
        {
            try
            {
                var timeToJoinPlast = registeredOn.AddYears(1) - DateTime.Now;
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(userId);
                    await _userManager.AddToRoleAsync(us, "Пластун");
                    return TimeSpan.Zero;
                }
                return timeToJoinPlast;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            throw new NotImplementedException();
        }

        public void Update(UserDTO user,IFormFile file)
        {
            var userForUpdate=_mapper.Map<UserDTO, User>(user);
            UploadPhoto(ref userForUpdate, file);

            throw new NotImplementedException();
        }
        private void UploadPhoto(ref User user,IFormFile file)
        {
            var userId = user.Id;
            var oldImageName = _repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault().ImagePath;
            if (file != null && file.Length > 0)
            {
                var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_env.WebRootPath, "images\\Users");
                if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                img.Save(filePath);
                user.ImagePath = fileName;
            }
            else
            {
                user.ImagePath = oldImageName;
            }
        }
    }
}
