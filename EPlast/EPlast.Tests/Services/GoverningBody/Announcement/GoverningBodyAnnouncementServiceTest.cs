using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.GoverningBody.Announcement
{
    internal class GoverningBodyAnnouncementServiceTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _context;
        private Mock<UserManager<User>> _userManager;
        private GoverningBodyAnnouncementService _governingBodyAnnouncementService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _context = new Mock<IHttpContextAccessor>();

            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _governingBodyAnnouncementService = new GoverningBodyAnnouncementService(
                _repoWrapper.Object,
                _mapper.Object,
                _context.Object,
                _userManager.Object);
        }

        [Test]
        public async Task AddAnnouncement_Valid()
        {
            //Arrange
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementDTO>(It.IsAny<GoverningBodyAnnouncement>()))
                .Returns(new GoverningBodyAnnouncementDTO());
            _repoWrapper
               .Setup(x => x.GoverningBodyAnnouncement.CreateAsync(It.IsAny<GoverningBodyAnnouncement>()));
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));

            //Act
            var result = await _governingBodyAnnouncementService.AddAnnouncementAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteAnnouncement_Valid()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(new GoverningBodyAnnouncement());

            //Act
            var result = _governingBodyAnnouncementService.DeleteAnnouncementAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void DeleteAnnouncement_ThrowException()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(nullGoverningBodyAnnouncement);

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(ArgumentNullException),
               async () => { await _governingBodyAnnouncementService.DeleteAnnouncementAsync(It.IsAny<int>()); });
            Assert.AreEqual("Value cannot be null. (Parameter 'Announcement with 0 not found')", exception.Message);
        }

        [Test]
        public async Task GetAllAnnouncement_Valid()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodyAnnouncement.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(GetTestPlastAnnouncement());
            _mapper.Setup(m => m.Map<IEnumerable<GoverningBodyAnnouncementUserDTO>>(It.IsAny<IEnumerable<GoverningBodyAnnouncement>>()))
                .Returns(GetTestPlastAnnouncementDTO());
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
               It.IsAny<Func<IQueryable<User>,
               IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var result = await _governingBodyAnnouncementService.GetAllAnnouncementAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAnnouncementById_Valid()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncement.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(new GoverningBodyAnnouncement());
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementUserDTO>(It.IsAny<GoverningBodyAnnouncement>()))
                .Returns(new GoverningBodyAnnouncementUserDTO());
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
              It.IsAny<Func<IQueryable<User>,
              IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var res = await _governingBodyAnnouncementService.GetAnnouncementByIdAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<GoverningBodyAnnouncementUserDTO>(res);
        }

        [Test]
        public async Task GetAllUserAsync_Valid()
        {
            //Arrange
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
              It.IsAny<Func<IQueryable<User>,
              IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var result = await _governingBodyAnnouncementService.GetAllUserAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        readonly GoverningBodyAnnouncement nullGoverningBodyAnnouncement = null;

        private IEnumerable<GoverningBodyAnnouncement> GetTestPlastAnnouncement()
        {
            return new List<GoverningBodyAnnouncement>
            {
                new GoverningBodyAnnouncement{Id = 1, Text = "За силу"},
                new GoverningBodyAnnouncement{Id = 2, Text = "За волю"},
                new GoverningBodyAnnouncement{Id = 3, Text = "За народ"}
            }.AsEnumerable();
        }

        private IEnumerable<GoverningBodyAnnouncementUserDTO> GetTestPlastAnnouncementDTO()
        {
            return new List<GoverningBodyAnnouncementUserDTO>
            {
                new GoverningBodyAnnouncementUserDTO{Id = 1, Text = "За силу"},
                new GoverningBodyAnnouncementUserDTO{Id = 2, Text = "За волю"},
                new GoverningBodyAnnouncementUserDTO{Id = 3, Text = "За народ"}
            }.AsEnumerable();
        }
    }
}
