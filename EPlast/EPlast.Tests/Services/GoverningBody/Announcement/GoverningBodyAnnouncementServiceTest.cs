using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
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
        private Mock<IGoverningBodyBlobStorageRepository> _blobStorage;
        private Mock<IUniqueIdService> _uniqueId;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _context = new Mock<IHttpContextAccessor>();
            _blobStorage = new Mock<IGoverningBodyBlobStorageRepository>();
            _uniqueId = new Mock<IUniqueIdService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _governingBodyAnnouncementService = new GoverningBodyAnnouncementService(
                _repoWrapper.Object,
                _mapper.Object,
                _context.Object,
                _blobStorage.Object,
                _userManager.Object,
                _uniqueId.Object);
        }

        [Test]
        public async Task AddAnnouncement_EmptyImages_Valid()
        {
            //Arrange
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementWithImagesDTO, GoverningBodyAnnouncement>(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .Returns(GetGoverningBodyAnnouncement());
            _repoWrapper
               .Setup(x => x.GoverningBodyAnnouncement.CreateAsync(It.IsAny<GoverningBodyAnnouncement>()));
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context.Setup(c => c.HttpContext.User).Returns(new ClaimsPrincipal());

            //Act
            var result = await _governingBodyAnnouncementService.AddAnnouncementAsync(GetGoverningBodyAnnouncementWithEmptyImagesDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.DoesNotThrowAsync(async () => {
                await _governingBodyAnnouncementService.AddAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>());
            });
        }

        [Test]
        public async Task AddAnnouncement_Valid()
        {
            //Arrange
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementWithImagesDTO, GoverningBodyAnnouncement>(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>()))
                .Returns(GetGoverningBodyAnnouncement());
            _repoWrapper
               .Setup(x => x.GoverningBodyAnnouncement.CreateAsync(It.IsAny<GoverningBodyAnnouncement>()));
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context.Setup(c => c.HttpContext.User).Returns(new ClaimsPrincipal());

            //Act
            var result = await _governingBodyAnnouncementService.AddAnnouncementAsync(GetNewAnnouncementWithImagesDto());

            //Assert
            Assert.IsNotNull(result);
            Assert.DoesNotThrowAsync(async () => {
                await _governingBodyAnnouncementService.AddAnnouncementAsync(It.IsAny<GoverningBodyAnnouncementWithImagesDTO>());
            });
        }

        [Test]
        public async Task AddAnnouncementAsync_TextIsNull_ReturnsNull()
        {
            //Arrange
            _mapper
                .Setup(m => m.Map<GoverningBodyAnnouncementDTO>(It.IsAny<GoverningBodyAnnouncement>()))
                .Returns(new GoverningBodyAnnouncementDTO());
            _repoWrapper
               .Setup(x => x.GoverningBodyAnnouncement.CreateAsync(It.IsAny<GoverningBodyAnnouncement>()));
            _userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));

            //Act
            int? result = await _governingBodyAnnouncementService.AddAnnouncementAsync(null);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteAnnouncement_Valid()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(GetGoverningBodyAnnouncement());

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
        public async Task GetAnnouncementById_Valid()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IIncludableQueryable<GoverningBodyAnnouncement, object>>>()))
                .ReturnsAsync(GetGoverningBodyAnnouncement());
            _mapper.Setup(m => m.Map<GoverningBodyAnnouncementUserDTO>(It.IsAny<GoverningBodyAnnouncement>()))
                .Returns(GetGoverningBodyAnnouncementUserDTO());
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
              It.IsAny<Func<IQueryable<User>,
              IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var res = await _governingBodyAnnouncementService.GetAnnouncementByIdAsync(228);

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<GoverningBodyAnnouncementUserDTO>(res);
        }

        [Test]
        public async Task GetAllUserAsync_Valid()
        {
            //Arrange
            _repoWrapper.Setup(u => u.User.GetAllAsync(null, null)).ReturnsAsync(GetTestPlastUsers());
            _mapper.Setup(m => m.Map<IEnumerable<User>, IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>()))
                .Returns(GetTestPlastUsersDTO());

            //Act
            var result = await _governingBodyAnnouncementService.GetAllUserAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<string>>(result);
        }

        [Test]
        public async Task EditAnnouncement_ReturnsId()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncement.Update(It.IsAny<GoverningBodyAnnouncement>()));
            _repoWrapper
                .Setup(x => x.GoverningBodyAnnouncementImage.Delete(It.IsAny<GoverningBodyAnnouncementImage>()));
            _repoWrapper.Setup(x => x.GoverningBodyAnnouncement.GetFirstAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
              It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>,
              IIncludableQueryable<GoverningBodyAnnouncement, object>>>())).ReturnsAsync(GetGoverningBodyAnnouncement());
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context
                .Setup(x => x.HttpContext.User)
                .Returns(new ClaimsPrincipal());

            //Act
            var result = await _governingBodyAnnouncementService.EditAnnouncementAsync(GetExistingAnnouncementWithImagesDto());

            //Assert
            Assert.IsNotNull(result);
            _repoWrapper.Verify(x => x.GoverningBodyAnnouncement.Update(It.IsAny<GoverningBodyAnnouncement>()));
            _repoWrapper.Verify(x => x.SaveAsync());
        }

        [TestCase(null)]
        public async Task EditAnnouncement_ReceivesNull_ReturnsNull(GoverningBodyAnnouncementWithImagesDTO announcementDTO)
        {
            //Act
            var result = await _governingBodyAnnouncementService.EditAnnouncementAsync(announcementDTO);

            //Assert
            Assert.IsNull(result);
        }

        [TestCase(1, 5, 1)]
        public async Task GetAnnouncementsByPage_ReturnsAnnouncements(int pageNumber, int pageSize, int governingBodyId)
        {
            //Arrange
            _repoWrapper
              .Setup(r => r.GoverningBodyAnnouncement.GetRangeAsync(It.IsAny<Expression<Func<GoverningBodyAnnouncement, bool>>>(),
              It.IsAny<Expression<Func<GoverningBodyAnnouncement, GoverningBodyAnnouncement>>>(),
              It.IsAny<Func<IQueryable<GoverningBodyAnnouncement>, IQueryable<GoverningBodyAnnouncement>>>(), null,
              It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);
            _repoWrapper
                .Setup(r => r.GoverningBodyAnnouncementImage.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<GoverningBodyAnnouncementImage, bool>>>(), null));
            _mapper
              .Setup(m => m.Map<IEnumerable<GoverningBodyAnnouncement>, IEnumerable<GoverningBodyAnnouncementUserDTO>>
              (It.IsAny<IEnumerable<GoverningBodyAnnouncement>>()))
              .Returns(GetTestPlastAnnouncementDTO());

            //Act
            var result = await _governingBodyAnnouncementService.GetAnnouncementsByPageAsync(pageNumber, pageSize, governingBodyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>>(result);
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

        private IEnumerable<User> GetTestPlastUsers()
        {
            return new List<User>
            {
                new User{Id = "1"},
                new User{Id = "2"},
                new User{Id = "3"},
            }.AsEnumerable();
        }

        private IEnumerable<UserDTO> GetTestPlastUsersDTO()
        {
            return new List<UserDTO>
            {
                new UserDTO{Id = "1"},
                new UserDTO{Id = "2"},
                new UserDTO{Id = "3"},
            }.AsEnumerable();
        }

        private GoverningBodyAnnouncementWithImagesDTO GetExistingAnnouncementWithImagesDto()
        {
            return new GoverningBodyAnnouncementWithImagesDTO
            {
                Id = 228,
                Text = "Hello world",
                ImagesBase64 = new List<string> { "data:image/jpg;base64,/9j/4AAQSk" }
            };
        }

        private GoverningBodyAnnouncementWithImagesDTO GetGoverningBodyAnnouncementWithEmptyImagesDTO()
        {
            return new GoverningBodyAnnouncementWithImagesDTO
            {
                Text = "Hello world",
                ImagesBase64 = new List<string> { "" }
            };
        }

        private GoverningBodyAnnouncementWithImagesDTO GetNewAnnouncementWithImagesDto()
        {
            return new GoverningBodyAnnouncementWithImagesDTO
            {
                Text = "Hello world",
                ImagesBase64 = new List<string> { "data:image/jpg;base64,/9j/4AAQSk" }
            };
        }

        private List<GoverningBodyAnnouncement> GetAnnouncementsByPage()
        {
            return new List<GoverningBodyAnnouncement>()
            {
                new GoverningBodyAnnouncement()
                {
                    Text = "Hello world"
                }
            };
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
                }
            };
        }

        private GoverningBodyAnnouncementUserDTO GetGoverningBodyAnnouncementUserDTO()
        {
            return new GoverningBodyAnnouncementUserDTO
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

        private Tuple<IEnumerable<GoverningBodyAnnouncement>, int> CreateTuple =>
            new Tuple<IEnumerable<GoverningBodyAnnouncement>, int>(GetAnnouncementsByPage(), 100);
    }
}
