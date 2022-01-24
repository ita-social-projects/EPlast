using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Distinctions;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.DistinctionServiceTest
{
    [TestFixture]
    class UserDistinctionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private UserDistinctionService distinctionService;
        private Mock<UserManager<User>> userManager;
        private IUniqueIdService _uniqueId;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _uniqueId = new UniqueIdService();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            distinctionService = new UserDistinctionService(mockMapper.Object, mockRepoWrapper.Object, userManager.Object);
        }

        [Test]
        public void DeleteUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeleteUserDistinctionAsync_IfAdmin_ThrowsNotImplementedException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(() => null);

            //Act

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(NotImplementedException),
                async () => { await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("The method or operation is not implemented.", exception.Message);
        }

        [Test]
        public void DeleteUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(userDistinction);

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () => { await distinctionService.DeleteUserDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangeUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.ChangeUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.GetFirstAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(userDistinction);

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await distinctionService.ChangeUserDistinctionAsync(userDistinctionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddUserDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.CreateAsync(It.IsAny<UserDistinction>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.AddUserDistinctionAsync(It.IsAny<UserDistinctionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddUserDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserDistinction.CreateAsync(userDistinction));

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await distinctionService.AddUserDistinctionAsync(userDistinctionDTO, It.IsAny<User>()); });
        }

        [Test]
        public async Task IsNumberExistAsync_True()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_False()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsNumberExistAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());

            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);

            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny <string>());

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);

            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserDistinctionAsync_IsNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.GetUserDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserDistinctionAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(userDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinction, UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns((UserDistinction src) => new UserDistinctionDTO() { Id = src.Id });

            //Act
            var result = await distinctionService.GetUserDistinctionAsync(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserDistinctionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(userDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinction, UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns((UserDistinction src) => new UserDistinctionDTO() { Id = src.Id });

            //Act
            var result = await distinctionService.GetUserDistinctionAsync(1);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UserDistinctionDTO>(result);
        }

        readonly UserDistinction nullDistinction = null;
        readonly UserDistinctionDTO nullDistinctionDTO = null;
        readonly List<UserDistinction> nulluserDistinctions = null;
        readonly List<UserDistinctionDTO> nulluserDistinctionsDTO = null;
                
        private string UserId => _uniqueId.GetUniqueId().ToString();

        private UserDistinction userDistinction => new UserDistinction
        {
            Id = 1,
            Distinction = new Distinction { Id = 1, Name = "За силу",
                                            UserDistinctions = new List<UserDistinction>() { new UserDistinction() } },
            UserId = UserId,
            Date = DateTime.Now,
            User = new User { FirstName = "", LastName = "", FatherName = "",
                                                  PhoneNumber = "", Id = UserId, ImagePath = "", Email = "", UserName = "",
                                                  UserDistinctions = new List<UserDistinction>(),
                                                  UserProfile = new UserProfile() },
            DistinctionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""            
        };

        private UserDistinctionDTO userDistinctionDTO => new UserDistinctionDTO
        {
            Id = 1,
            Distinction = new DistinctionDTO { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName = "",
                                                  Email = "", ID = UserId, ImagePath = "", PhoneNumber = "" },
            DistinctionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""
        };

        private IEnumerable<UserDistinction> GetTestUserDistinction()
        {
            return new List<UserDistinction>
            {
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinction
               {
                   Distinction = new Distinction{Id = 3, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               }
            }.AsEnumerable();
        }

        private IEnumerable<UserDistinctionDTO> GetTestUserDistinctionDTO()
        {
            return new List<UserDistinctionDTO>
            {
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserDistinctionDTO
               {
                   Distinction = new DistinctionDTO{Id = 3, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               }
            }.AsEnumerable();
        }
        private IList<string> GetRoles()
        {
            return new List<string>
            {
                Roles.Admin,
                "Htos",
                "Nixto"

            };
        }
        private IList<string> GetRolesWithoutAdmin()
        {
            return new List<string>
            {
                "Htos",
                "Nixto"

            };
        }
    }
}
