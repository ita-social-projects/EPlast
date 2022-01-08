using AutoMapper;
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
using System.Linq.Expressions;

namespace EPlast.BLL.Services.GoverningBodies.Announcement
{
    public class GoverningBodyAnnouncementService : IGoverningBodyAnnouncementService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;

        public GoverningBodyAnnouncementService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper, IHttpContextAccessor context, UserManager<User> userManager)
        {
            _repoWrapper = repositoryWrapper;
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

        [Obsolete("This method is obsolete. Use GetAnnouncementsByPageAsync method to provide better performance")]
        public async Task<IEnumerable<GoverningBodyAnnouncementUserDTO>> GetAllAnnouncementAsync()
        {
            var announcements = _mapper.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>(await _repoWrapper.GoverningBodyAnnouncement.GetAllAsync());
            foreach (GoverningBodyAnnouncementUserDTO announcement in announcements)
            {
                var user = await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId);
                announcement.User = _mapper.Map<UserDTO>(user);
            }
            return announcements.OrderByDescending(d => d.Date);
        }

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize)
        {
            var order = GetOrder();
            var selector = GetSelector();
            var tuple = await _repoWrapper.GoverningBodyAnnouncement.GetRangeAsync(null, selector, order, pageNumber, pageSize, true);
            var clubs = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>(_mapper.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>(clubs), rows);
        }

        public async Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementByIdAsync(int id)
        {
            var announcement = _mapper.Map<GoverningBodyAnnouncementUserDTO>(await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(d => d.Id == id));

            var user = await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId);
            announcement.User = _mapper.Map<UserDTO>(user);
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
        public async Task<int?> EditAnnouncement(int id, string text)
        {
            GoverningBodyAnnouncement updatedAnnouncement = await _repoWrapper.GoverningBodyAnnouncement.GetFirstOrDefaultAsync(x=>x.Id == id);
            if (updatedAnnouncement == null) return null;
            updatedAnnouncement.Text = text;
            _repoWrapper.GoverningBodyAnnouncement.Update(updatedAnnouncement);
            await _repoWrapper.SaveAsync();
            return updatedAnnouncement.Id;
        }

        private Expression<Func<GoverningBodyAnnouncement, object>> GetOrder()
        {
            Expression<Func<GoverningBodyAnnouncement, object>> expr = x => x.Date;
            return expr;
        }
        private Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>> GetSelector()
        {

            Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>> expr = x => 
            new GoverningBodyAnnouncement { 
                Id = x.Id,
                UserId = x.UserId,
                Text = x.Text, 
                User = new User
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    ImagePath = x.User.ImagePath
                }, 
                Date = x.Date };
            return expr;
        }
    }
}
