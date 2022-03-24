using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
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

namespace EPlast.Tests.Services.GoverningBody.Sector
{
    internal class SectorAnnouncementsServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _context;
        private Mock<UserManager<User>> _userManager;
        private SectorAnnouncementsService _sectorAnnouncementService;
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

            _sectorAnnouncementService = new SectorAnnouncementsService(
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
            _mapper.Setup(m => m.Map<SectorAnnouncementWithImagesDTO, SectorAnnouncement>(It.IsAny<SectorAnnouncementWithImagesDTO>()))
                .Returns(GetSectorAnnouncement());
            _repoWrapper
               .Setup(x => x.GoverningBodySectorAnnouncements.CreateAsync(It.IsAny<SectorAnnouncement>()));
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context.Setup(c => c.HttpContext.User).Returns(new ClaimsPrincipal());

            //Act
            var result = await _sectorAnnouncementService.AddAnnouncementAsync(GetSectorAnnouncementWithEmptyImagesDTO());

            //Assert
            Assert.IsNotNull(result);
            Assert.DoesNotThrowAsync(async () => {
                await _sectorAnnouncementService.AddAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>());
            });
        }

        [Test]
        public async Task AddAnnouncement_Valid()
        {
            //Arrange
            _mapper.Setup(m => m.Map<SectorAnnouncementWithImagesDTO, SectorAnnouncement>(It.IsAny<SectorAnnouncementWithImagesDTO>()))
                .Returns(GetSectorAnnouncement());
            _repoWrapper
               .Setup(x => x.GoverningBodySectorAnnouncements.CreateAsync(It.IsAny<SectorAnnouncement>()));
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context.Setup(c => c.HttpContext.User).Returns(new ClaimsPrincipal());

            //Act
            var result = await _sectorAnnouncementService.AddAnnouncementAsync(GetNewAnnouncementWithImagesDto());

            //Assert
            Assert.IsNotNull(result);
            Assert.DoesNotThrowAsync(async () => {
                await _sectorAnnouncementService.AddAnnouncementAsync(It.IsAny<SectorAnnouncementWithImagesDTO>());
            });
        }

        [Test]
        public async Task AddAnnouncementAsync_TextIsNull_ReturnsNull()
        {
            //Arrange
            _mapper
                .Setup(m => m.Map<SectorAnnouncementDTO>(It.IsAny<SectorAnnouncement>()))
                .Returns(new SectorAnnouncementDTO());
            _repoWrapper
               .Setup(x => x.GoverningBodySectorAnnouncements.CreateAsync(It.IsAny<SectorAnnouncement>()));
            _userManager
                .Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));

            //Act
            int? result = await _sectorAnnouncementService.AddAnnouncementAsync(null);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteAnnouncement_Valid()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodySectorAnnouncements.GetFirstAsync(It.IsAny<Expression<Func<SectorAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<SectorAnnouncement>, IIncludableQueryable<SectorAnnouncement, object>>>()))
                .ReturnsAsync(GetSectorAnnouncement());

            //Act
            var result = _sectorAnnouncementService.DeleteAnnouncementAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteAnnouncement_ThrowException()
        {
            //Arrange
            _repoWrapper.Setup(g => g.GoverningBodySectorAnnouncements.GetFirstAsync(It.IsAny<Expression<Func<SectorAnnouncement, bool>>>(),
                   It.IsAny<Func<IQueryable<SectorAnnouncement>, IIncludableQueryable<SectorAnnouncement, object>>>()))
                .ReturnsAsync(nullSectorAnnouncement);

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(ArgumentNullException),
               async () => { await _sectorAnnouncementService.DeleteAnnouncementAsync(It.IsAny<int>()); });
            Assert.AreEqual("Value cannot be null. (Parameter 'Announcement with 0 not found')", exception.Message);
        }

        [Test]
        public async Task GetAnnouncementById_Valid()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAnnouncements.GetFirstAsync(It.IsAny<Expression<Func<SectorAnnouncement, bool>>>(),
                    It.IsAny<Func<IQueryable<SectorAnnouncement>, IIncludableQueryable<SectorAnnouncement, object>>>()))
                .ReturnsAsync(GetSectorAnnouncement());
            _mapper.Setup(m => m.Map<SectorAnnouncementUserDTO>(It.IsAny<SectorAnnouncement>()))
                .Returns(GetSectorAnnouncementUserDTO());
            var a = _repoWrapper.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
              It.IsAny<Func<IQueryable<User>,
              IIncludableQueryable<User, object>>>())).ReturnsAsync(new User());
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(a));

            //Act
            var res = await _sectorAnnouncementService.GetAnnouncementByIdAsync(228);

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<SectorAnnouncementUserDTO>(res);
        }

        [Test]
        public async Task GetAllUserAsync_Valid()
        {
            //Arrange
            _repoWrapper.Setup(u => u.User.GetAllAsync(null, null)).ReturnsAsync(GetTestPlastUsers());
            _mapper.Setup(m => m.Map<IEnumerable<User>, IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>()))
                .Returns(GetTestPlastUsersDTO());

            //Act
            var result = await _sectorAnnouncementService.GetAllUserAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<string>>(result);
        }

        [Test]
        public async Task EditAnnouncement_ReturnsId()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAnnouncements.Update(It.IsAny<SectorAnnouncement>()));
            _repoWrapper
                .Setup(x => x.GoverningBodySectorAnnouncementImage.Delete(It.IsAny<SectorAnnouncementImage>()));
            _repoWrapper.Setup(x => x.GoverningBodySectorAnnouncements.GetFirstAsync(It.IsAny<Expression<Func<SectorAnnouncement, bool>>>(),
              It.IsAny<Func<IQueryable<SectorAnnouncement>,
              IIncludableQueryable<SectorAnnouncement, object>>>())).ReturnsAsync(GetSectorAnnouncement());
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()));
            _context
                .Setup(x => x.HttpContext.User)
                .Returns(new ClaimsPrincipal());

            //Act
            var result = await _sectorAnnouncementService.EditAnnouncementAsync(GetExistingAnnouncementWithImagesDto());

            //Assert
            Assert.IsNotNull(result);
            _repoWrapper.Verify(x => x.GoverningBodySectorAnnouncements.Update(It.IsAny<SectorAnnouncement>()));
            _repoWrapper.Verify(x => x.SaveAsync());
        }

        [TestCase(null)]
        public async Task EditAnnouncement_ReceivesNull_ReturnsNull(SectorAnnouncementWithImagesDTO announcementDTO)
        {
            //Act
            var result = await _sectorAnnouncementService.EditAnnouncementAsync(announcementDTO);

            //Assert
            Assert.IsNull(result);
        }

        [TestCase(1, 5, 1)]
        public async Task GetAnnouncementsByPage_ReturnsAnnouncements(int pageNumber, int pageSize, int governingBodyId)
        {
            //Arrange
            _repoWrapper
              .Setup(r => r.GoverningBodySectorAnnouncements.GetRangeAsync(It.IsAny<Expression<Func<SectorAnnouncement, bool>>>(),
              It.IsAny<Expression<Func<SectorAnnouncement, SectorAnnouncement>>>(),
              It.IsAny<Func<IQueryable<SectorAnnouncement>, IQueryable<SectorAnnouncement>>>(), null,
              It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);
            _repoWrapper
                .Setup(r => r.GoverningBodySectorAnnouncementImage.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<SectorAnnouncementImage, bool>>>(), null));
            _mapper
              .Setup(m => m.Map<IEnumerable<SectorAnnouncement>, IEnumerable<SectorAnnouncementUserDTO>>
              (It.IsAny<IEnumerable<SectorAnnouncement>>()))
              .Returns(GetTestPlastAnnouncementDTO());

            //Act
            var result = await _sectorAnnouncementService.GetAnnouncementsByPageAsync(pageNumber, pageSize, governingBodyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<SectorAnnouncementUserDTO>, int>>(result);
        }

        readonly SectorAnnouncement nullSectorAnnouncement = null;

        private IEnumerable<SectorAnnouncement> GetTestPlastAnnouncement()
        {
            return new List<SectorAnnouncement>
            {
                new SectorAnnouncement{Id = 1, Text = "За силу"},
                new SectorAnnouncement{Id = 2, Text = "За волю"},
                new SectorAnnouncement{Id = 3, Text = "За народ"}
            }.AsEnumerable();
        }

        private IEnumerable<SectorAnnouncementUserDTO> GetTestPlastAnnouncementDTO()
        {
            return new List<SectorAnnouncementUserDTO>
            {
                new SectorAnnouncementUserDTO{Id = 1, Text = "За силу"},
                new SectorAnnouncementUserDTO{Id = 2, Text = "За волю"},
                new SectorAnnouncementUserDTO{Id = 3, Text = "За народ"}
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

        private SectorAnnouncementWithImagesDTO GetExistingAnnouncementWithImagesDto()
        {
            return new SectorAnnouncementWithImagesDTO
            {
                Id = 228,
                Text = "Hello world",
                ImagesBase64 = new List<string> { "data:image/jpg;base64,/9j/4AAQSk" }
            };
        }

        private SectorAnnouncementWithImagesDTO GetSectorAnnouncementWithEmptyImagesDTO()
        {
            return new SectorAnnouncementWithImagesDTO
            {
                Text = "Hello world",
                ImagesBase64 = new List<string> { "" }
            };
        }

        private SectorAnnouncementWithImagesDTO GetNewAnnouncementWithImagesDto()
        {
            return new SectorAnnouncementWithImagesDTO
            {
                Text = "Hello world",
                ImagesBase64 = new List<string> { "data:image/jpg;base64,/9j/4AAQSk" }
            };
        }

        private List<SectorAnnouncement> GetAnnouncementsByPage()
        {
            return new List<SectorAnnouncement>()
            {
                new SectorAnnouncement()
                {
                    Text = "Hello world"
                }
            };
        }

        private SectorAnnouncement GetSectorAnnouncement()
        {
            return new SectorAnnouncement
            {
                Id = 1,
                Text = "Hello world",
                Images = new List<SectorAnnouncementImage> {
                    new SectorAnnouncementImage
                    {
                        ImagePath = "image.png"
                    }
                }
            };
        }

        private SectorAnnouncementUserDTO GetSectorAnnouncementUserDTO()
        {
            return new SectorAnnouncementUserDTO
            {
                Images = new List<SectorAnnouncementImageDTO>()
                {
                    new SectorAnnouncementImageDTO
                    {
                        ImagePath = "image.png"
                    }
                }
            };
        }

        private Tuple<IEnumerable<SectorAnnouncement>, int> CreateTuple =>
            new Tuple<IEnumerable<SectorAnnouncement>, int>(GetAnnouncementsByPage(), 100);
    }
}
