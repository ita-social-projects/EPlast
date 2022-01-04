using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.Interfaces.UserProfiles;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies.Announcement
{
    public class GoverningBodyAnnouncementService : IGoverningBodyAnnouncementService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;

        public GoverningBodyAnnouncementService(IRepositoryWrapper repositoryWrapper,
            IUserService userService, IMapper mapper, 
            IHttpContextAccessor context, UserManager<User> userManager)
        {
            _repoWrapper = repositoryWrapper;
            _userService = userService;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<int?> AddAnnouncementAsync(string text)
        {
            if (text == null)
            {
                return null;
            }
            var governingBodyAnnouncementDTO = new GoverningBodyAnnouncementDTO();
            governingBodyAnnouncementDTO.Text = text;
            governingBodyAnnouncementDTO.UserId = _userManager.GetUserId(_context.HttpContext.User);
            var announcement = _mapper.Map<GoverningBodyAnnouncementDTO, GoverningBodyAnnouncement>(governingBodyAnnouncementDTO);
            announcement.Date = DateTime.Now;
            await _repoWrapper.GoverningBodyAnnouncement.CreateAsync(announcement);
            await _repoWrapper.SaveAsync();
            return announcement.Id;
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var announcement = (await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(d => d.Id == id));
            if (announcement == null)
            {
                throw new ArgumentNullException($"Announcement with {id} not found");
            }
            _repoWrapper.GoverningBodyAnnouncement.Delete(announcement);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<GoverningBodyAnnouncementUserDTO>> GetAllAnnouncementAsync()
        {
            var announcements = _mapper.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>(await _repoWrapper.GoverningBodyAnnouncement.GetAllAsync());
            foreach (GoverningBodyAnnouncementUserDTO announcement in announcements)
            {
                announcement.User = _mapper.Map<UserDTO>(await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId));
                announcement.ProfileImageBase64 = await _userService.GetProfileImageAsync(announcement.UserId);
            }
            return announcements.OrderByDescending(d => d.Date);
        }

        public async Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementByIdAsync(int id)
        {
            var announcement = _mapper.Map<GoverningBodyAnnouncementUserDTO>(await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(d => d.Id == id));
            announcement.User = _mapper.Map<UserDTO>(await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId));
            announcement.ProfileImageBase64 = await _userService.GetProfileImageAsync(announcement.UserId);
            return announcement;
        }

        public async Task<List<string>> GetAllUserAsync()
        {
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _repoWrapper.User.GetAllAsync());
            var userIds =  new List<string>();
            foreach (var user in users)
            {
                userIds.Add(user.Id);
            }

            return userIds;
        }
        public async Task<int> EditAnnouncement(GoverningBodyAnnouncementUserDTO announcement)
        {
            GoverningBodyAnnouncement updatedAnnouncement = new GoverningBodyAnnouncement()
            {
                Id = announcement.Id,
                Date = announcement.Date,
                UserId = announcement.UserId,
                User = _mapper.Map<UserDTO, User>(announcement.User),
                Text = announcement.Text
            };
            _repoWrapper.GoverningBodyAnnouncement.Update(updatedAnnouncement);
            await _repoWrapper.SaveAsync();
            return updatedAnnouncement.Id;
        }
    }
}
