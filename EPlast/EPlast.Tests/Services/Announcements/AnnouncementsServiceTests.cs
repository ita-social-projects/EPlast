using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Services.Announcements;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Announcements
{
    internal class AnnouncementsServiceTests
    {
        private AnnouncementsService _announcementService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IGoverningBodyBlobStorageService> _blobStorageService;


        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobStorageService = new Mock<IGoverningBodyBlobStorageService>();

            _announcementService = new AnnouncementsService(
                _repoWrapper.Object,
                _mapper.Object,
                _blobStorageService.Object
               );
        }

        [Test]
        public async Task GetAnnouncementsByPageAsync_ReturnsTuple()
        {
            //Arrange
            _repoWrapper.Setup(r => r.GoverningBodyAnnouncement.GetRangeAsync(
                    It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                    It.IsAny<Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IQueryable<GoverningBodyAnnouncement>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>,
                        IIncludableQueryable<GoverningBodyAnnouncement, object>>>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(
                    new Tuple<IEnumerable<GoverningBodyAnnouncement>, int>(new List<GoverningBodyAnnouncement>(),
                        It.IsAny<int>()));
            _mapper.Setup(m =>
                m.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>(
                    It.IsAny<IEnumerable<GoverningBodyAnnouncement>>()));

            //Act
            var result =
                await _announcementService.GetAnnouncementsByPageAsync(It.IsAny<int>(),
                    It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>>(result);
        }

        [Test]
        public async Task GetAnnouncementById_Valid()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(GetGoverningBodyAnnouncement());
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementUserWithImagesDTO>(It.IsAny<GoverningBodyAnnouncement>()))
                .Returns(GetGoverningBodyAnnouncementUserDTO());
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
              It.IsAny<Func<IQueryable<User>,
              IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var res = await _announcementService.GetAnnouncementByIdAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<GoverningBodyAnnouncementUserWithImagesDTO>(res);
        }


        [Test]
        public async Task PinAnnouncement_Valid()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(GetGoverningBodyAnnouncement());

            _repoWrapper
              .Setup(x => x.GoverningBodyAnnouncement.Update(It.IsAny<GoverningBodyAnnouncement>()));
            //Act
            var res = await _announcementService.PinAnnouncementAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<int>(res);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Once);
            _repoWrapper.Verify(x => x.GoverningBodyAnnouncement.Update(It.IsAny<GoverningBodyAnnouncement>()), Times.Once);
        }


        private GoverningBodyAnnouncement GetGoverningBodyAnnouncement()
        {
            return new GoverningBodyAnnouncement
            {
                Id = 1,
                Text = "Hello world",
                Images = new List<GoverningBodyAnnouncementImage> {
                    new GoverningBodyAnnouncementImage
                    {
                        ImagePath = "image.png"
                    }
                },
                IsPined = true
            };
        }

        private GoverningBodyAnnouncementUserWithImagesDTO GetGoverningBodyAnnouncementUserDTO()
        {
            return new GoverningBodyAnnouncementUserWithImagesDTO
            {
                Images = new List<GoverningBodyAnnouncementImageDTO>()
                {
                    new GoverningBodyAnnouncementImageDTO
                    {
                        ImagePath = "image.png"
                    }
                }
            };
        }
    }
}
