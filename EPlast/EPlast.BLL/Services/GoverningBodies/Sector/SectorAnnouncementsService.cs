using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorAnnouncementsService : ISectorAnnouncementsService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly IGoverningBodyBlobStorageRepository _blobStorage;
        private readonly UserManager<User> _userManager;
        private readonly IGoverningBodyBlobStorageService _blobStorageService;
        private readonly IHtmlService _htmlService;

        public SectorAnnouncementsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper, IHttpContextAccessor context,
            IGoverningBodyBlobStorageRepository blobStorage,
            UserManager<User> userManager, IGoverningBodyBlobStorageService blobStorageService, IHtmlService htmlService)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _context = context;
            _blobStorage = blobStorage;
            _userManager = userManager;
            _blobStorageService = blobStorageService;
            _htmlService = htmlService;
        }

        public async Task<int?> AddAnnouncementAsync(SectorAnnouncementWithImagesDTO announcementDTO)
        {
            if (announcementDTO == null)
            {
                return null;
            }
            if (_htmlService.IsHtmlTextEmpty(announcementDTO.Text)
               || _htmlService.IsHtmlTextEmpty(announcementDTO.Title))
            {
                return null;
            }
            announcementDTO.UserId = _userManager.GetUserId(_context.HttpContext.User);
            var announcement = _mapper.Map<SectorAnnouncementWithImagesDTO, SectorAnnouncement>(announcementDTO);
            announcement.Images = new List<SectorAnnouncementImage>();
            foreach (var image in announcementDTO.ImagesBase64)
            {
                announcement.Images.Add(new SectorAnnouncementImage { ImagePath = await _blobStorageService.UploadImageAsync(image) });
            }
            announcement.Date = DateTime.Now;
            await _repoWrapper.GoverningBodySectorAnnouncements.CreateAsync(announcement);
            await _repoWrapper.SaveAsync();
            return announcement.Id;
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var announcement = await _repoWrapper.GoverningBodySectorAnnouncements.GetFirstAsync(
                d => d.Id == id,
                src => src.Include(g => g.Images));
            if (announcement == null)
            {
                throw new ArgumentNullException($"Announcement with {id} not found");
            }
            foreach (var image in announcement.Images)
            {
                await _blobStorage.DeleteBlobAsync(image.ImagePath);
            }
            _repoWrapper.GoverningBodySectorAnnouncements.Delete(announcement);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<SectorAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize, int sectorId)
        {
            var order = GetOrder();
            var selector = GetSelector();

            var tuple = await _repoWrapper.GoverningBodySectorAnnouncements.GetRangeAsync(x => x.SectorId == sectorId, selector, order, null, pageNumber, pageSize);
            var announcements = _mapper.Map<IEnumerable<SectorAnnouncement>, IEnumerable<SectorAnnouncementUserDTO>>(tuple.Item1);

            foreach (var ann in announcements)
            {
                ann.ImagesPresent =
                    await _repoWrapper.GoverningBodySectorAnnouncementImage.GetFirstOrDefaultAsync(i => i.SectorAnnouncementId == ann.Id)
                    != null;
            }
            var rows = announcements.Count();

            return new Tuple<IEnumerable<SectorAnnouncementUserDTO>, int>
                (announcements, rows);
        }

        public async Task<SectorAnnouncementUserDTO> GetAnnouncementByIdAsync(int id)
        {
            var announcement = _mapper.Map<SectorAnnouncementUserDTO>(
                await _repoWrapper.GoverningBodySectorAnnouncements.GetFirstAsync(
                    d => d.Id == id,
                    src => src.Include(g => g.Images)));

            foreach (var image in announcement.Images)
            {
                image.ImageBase64 = await _blobStorageService.GetImageAsync(image.ImagePath);
            }

            var user = await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId);
            announcement.User = _mapper.Map<UserDTO>(user);
            return announcement;
        }

        public async Task<List<string>> GetAllUserAsync()
        {
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _repoWrapper.User.GetAllAsync());
            var userIds = new List<string>();
            foreach (var user in users)
            {
                userIds.Add(user.Id);
            }
            return userIds;
        }

        public async Task<int?> EditAnnouncementAsync(SectorAnnouncementWithImagesDTO announcementDTO)
        {
            if (announcementDTO == null)
            {
                return null;
            }
            if (_htmlService.IsHtmlTextEmpty(announcementDTO.Text)
              || _htmlService.IsHtmlTextEmpty(announcementDTO.Title))
            {
                return null;
            }
            announcementDTO.UserId = _userManager.GetUserId(_context.HttpContext.User);
            var currentAnnouncement = await _repoWrapper.GoverningBodySectorAnnouncements.GetFirstAsync(
                    d => d.Id == announcementDTO.Id,
                    src => src.Include(g => g.Images));

            foreach (var image in currentAnnouncement.Images)
            {
                await _blobStorage.DeleteBlobAsync(image.ImagePath);
                _repoWrapper.GoverningBodySectorAnnouncementImage.Delete(image);
            }
            await _repoWrapper.SaveAsync();
            currentAnnouncement.Images = new List<SectorAnnouncementImage>();
            foreach (var image in announcementDTO.ImagesBase64)
            {
                currentAnnouncement.Images.Add(new SectorAnnouncementImage
                {
                    ImagePath = await _blobStorageService.UploadImageAsync(image)
                });
            }
            currentAnnouncement.Text = announcementDTO.Text;
            currentAnnouncement.Title = announcementDTO.Title;
            currentAnnouncement.Date = DateTime.Now;
            _repoWrapper.GoverningBodySectorAnnouncements.Update(currentAnnouncement);
            await _repoWrapper.SaveAsync();
            return currentAnnouncement.Id;
        }

        private Func<IQueryable<SectorAnnouncement>, IQueryable<SectorAnnouncement>> GetOrder()
        {
            Func<IQueryable<SectorAnnouncement>, IQueryable<SectorAnnouncement>> expr = x =>
            x.OrderByDescending(y => y.Date);
            return expr;
        }
        private Expression<Func<SectorAnnouncement, SectorAnnouncement>> GetSelector()
        {

            Expression<Func<SectorAnnouncement, SectorAnnouncement>> expr = x =>
            new SectorAnnouncement
            {
                Id = x.Id,
                UserId = x.UserId,
                Text = x.Text,
                Title = x.Title,
                User = new User
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    ImagePath = x.User.ImagePath
                },
                SectorId = x.SectorId,
                Date = x.Date
            };
            return expr;
        }
    }
}
