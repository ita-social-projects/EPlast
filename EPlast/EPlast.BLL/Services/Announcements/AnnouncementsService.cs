using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.Announcements;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Announcements
{
    public class AnnouncementsService : IAnnouncemetsService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IGoverningBodyBlobStorageService _blobStorageService;
        public AnnouncementsService(IRepositoryWrapper repoWrapper, IMapper mapper, IGoverningBodyBlobStorageService blobStorageService)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }

        public async Task<GoverningBodyAnnouncementUserWithImagesDto> GetAnnouncementByIdAsync(int id)
        {
            var announcement = _mapper.Map<GoverningBodyAnnouncementUserWithImagesDto>(
                await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(
                    d => d.Id == id,
                    src => src.Include(g => g.Images)));

            foreach (var image in announcement.Images)
            {
                image.ImageBase64 = await _blobStorageService.GetImageAsync(image.ImagePath);
            }

            var user = await _repoWrapper.User.GetFirstOrDefaultAsync(d => d.Id == announcement.UserId);
            announcement.User = _mapper.Map<UserDto>(user);
            return announcement;
        }

        public async Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDto>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize)
        {
            var order = GetOrder();
            var selector = GetSelector();
            var tuple = await _repoWrapper.GoverningBodyAnnouncement.GetRangeAsync(
                predicate: null,
                selector: selector,
                sorting: order,
                null, pageNumber, pageSize
              );

            var announcements = _mapper.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDto>>(tuple.Item1);

            foreach (var ann in announcements)
            {
                ann.ImagesPresent =
                    await _repoWrapper.GoverningBodyAnnouncementImage.GetFirstOrDefaultAsync(i => i.GoverningBodyAnnouncementId == ann.Id)
                    != null;
            }
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<GoverningBodyAnnouncementUserDto>, int>
                (announcements, rows);
        }

        public async Task<int?> PinAnnouncementAsync(int id)
        {
            var currentAnnouncement = await _repoWrapper.GoverningBodyAnnouncement.GetFirstAsync(
                    d => d.Id == id,
                    src => src.Include(g => g.Images));
            currentAnnouncement.IsPined = !currentAnnouncement.IsPined;
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

            Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>> expr = selector =>
            new GoverningBodyAnnouncement
            {
                Id = selector.Id,
                UserId = selector.UserId,
                Title = selector.Title,
                Text = selector.Text,
                User = new User
                {
                    FirstName = selector.User.FirstName,
                    LastName = selector.User.LastName,
                    ImagePath = selector.User.ImagePath
                },
                GoverningBodyId = selector.GoverningBodyId,
                SectorId = selector.SectorId,
                IsPined = selector.IsPined,
                Date = selector.Date
            };
            return expr;
        }

    }
}
