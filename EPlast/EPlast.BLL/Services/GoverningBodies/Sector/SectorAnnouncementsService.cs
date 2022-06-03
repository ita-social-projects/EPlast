﻿using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
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

        public async Task<int?> AddAnnouncementAsync(GoverningBodyAnnouncementWithImagesDTO announcementDTO)
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
            var announcement = _mapper.Map<GoverningBodyAnnouncementWithImagesDTO, GoverningBodyAnnouncement>(announcementDTO);
            announcement.Images = new List<GoverningBodyAnnouncementImage>();
            foreach (var image in announcementDTO.ImagesBase64)
            {
                announcement.Images.Add(new GoverningBodyAnnouncementImage { ImagePath = await _blobStorageService.UploadImageAsync(image) });
            }
            announcement.Date = DateTime.Now;
            await _repoWrapper.GoverningBodyAnnouncement.CreateAsync(announcement);
            await _repoWrapper.SaveAsync();
            return announcement.Id;
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var announcement = await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(
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
            _repoWrapper.GoverningBodyAnnouncement.Delete(announcement);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize, int sectorId)
        {
            var order = GetOrder();
            var selector = GetSelector();

            var tuple = await _repoWrapper.GoverningBodyAnnouncement.GetRangeAsync(x => x.SectorId == sectorId, selector, order, null, pageNumber, pageSize);
            var announcements = _mapper.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>(tuple.Item1);

            foreach (var ann in announcements)
            {
                ann.ImagesPresent =
                    await _repoWrapper.GoverningBodyAnnouncementImage.GetFirstOrDefaultAsync(i => i.GoverningBodyAnnouncementId == ann.Id)
                    != null;
            }
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>
                (announcements, rows);
        }

        public async Task<GoverningBodyAnnouncementUserWithImagesDTO> GetAnnouncementByIdAsync(int id)
        {
            var announcement = _mapper.Map<GoverningBodyAnnouncementUserWithImagesDTO>(
                await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(
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

        public async Task<int?> EditAnnouncementAsync(GoverningBodyAnnouncementWithImagesDTO announcementDTO)
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
            var currentAnnouncement = await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(
                    d => d.Id == announcementDTO.Id,
                    src => src.Include(g => g.Images));

            foreach (var image in currentAnnouncement.Images)
            {
                await _blobStorage.DeleteBlobAsync(image.ImagePath);
                _repoWrapper.GoverningBodyAnnouncementImage.Delete(image);
            }
            await _repoWrapper.SaveAsync();
            currentAnnouncement.Images = new List<GoverningBodyAnnouncementImage>();
            foreach (var image in announcementDTO.ImagesBase64)
            {
                currentAnnouncement.Images.Add(new GoverningBodyAnnouncementImage
                {
                    ImagePath = await _blobStorageService.UploadImageAsync(image)
                });
            }
            currentAnnouncement.Text = announcementDTO.Text;
            currentAnnouncement.Title = announcementDTO.Title;
            _repoWrapper.GoverningBodyAnnouncement.Update(currentAnnouncement);
            await _repoWrapper.SaveAsync();
            return currentAnnouncement.Id;
        }

        private Func<IQueryable<GoverningBodyAnnouncement>, IQueryable<GoverningBodyAnnouncement>> GetOrder()
        {
            Func<IQueryable<GoverningBodyAnnouncement>, IQueryable<GoverningBodyAnnouncement>> expr = order =>
            order.OrderByDescending(y => y.IsPined).ThenByDescending(y => y.Date);
            return expr;
        }
        private Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>> GetSelector()
        {

            Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>> expr = x =>
            new GoverningBodyAnnouncement
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
                IsPined = x.IsPined,
                Date = x.Date
            };
            return expr;
        }
    }
}
