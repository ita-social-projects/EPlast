using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services;
using EPlast.BLL.Services.Precautions;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.BLL.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.Tests.Services.Precautions
{
    class UserPrecautionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private UserPrecautionService PrecautionService;
        private Mock<UserManager<User>> userManager;
        private Mock<IAdminService> adminService;
        private IUniqueIdService _uniqueId;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            adminService = new Mock<IAdminService>();
            _uniqueId = new UniqueIdService();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            PrecautionService = new UserPrecautionService(mockMapper.Object, mockRepoWrapper.Object, userManager.Object, adminService.Object);
        }

        [Test]
        public void DeleteUserPrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.GetFirstAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeleteUserPrecautionAsync_IfAdmin_ThrowsNotImplementedException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.GetFirstAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(() => null);

            //Act

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(NotImplementedException),
                async () => { await PrecautionService.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("The method or operation is not implemented.", exception.Message);
        }

        [Test]
        public void DeleteUserPrecautionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await PrecautionService.DeleteUserPrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangeUserPrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.GetFirstAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.ChangeUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeUserPrecautionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.GetFirstAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await PrecautionService.ChangeUserPrecautionAsync(userPrecautionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddUserPrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.CreateAsync(It.IsAny<UserPrecaution>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.AddUserPrecautionAsync(It.IsAny<UserPrecautionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddUserPrecautionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
                 .Setup(x => x.UserPrecaution.CreateAsync(userPrecaution));

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await PrecautionService.AddUserPrecautionAsync(userPrecautionDTO, It.IsAny<User>()); });
        }

        [Test]
        public async Task IsNumberExistAsync_True()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());
            mockMapper
                .Setup(m => m.Map<UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns(new UserPrecautionDTO());

            //Act
            var result = await PrecautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_False()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            mockMapper
                .Setup(m => m.Map<UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns(nullPrecautionDTO);

            //Act
            var result = await PrecautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsNumberExistAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());
            mockMapper
                .Setup(m => m.Map<UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns(new UserPrecautionDTO());

            //Act
            var result = await PrecautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(GetTestUserPrecaution());

            mockMapper
               .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await PrecautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserPrecautionDTO>>(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await PrecautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nulluserPrecautions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
                .Returns(nulluserPrecautionsDTO);

            //Act
            var result = await PrecautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await PrecautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserPrecautionDTO>>(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await PrecautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nulluserPrecautions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
                .Returns(nulluserPrecautionsDTO);

            //Act
            var result = await PrecautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            mockMapper
                .Setup(m => m.Map<UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns(nullPrecautionDTO);

            //Act
            var result = await PrecautionService.GetUserPrecautionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);
            mockMapper
                .Setup(m => m.Map<UserPrecaution, UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns((UserPrecaution src) => new UserPrecautionDTO() { Id = src.Id });

            //Act
            var result = await PrecautionService.GetUserPrecautionAsync(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);
            mockMapper
                .Setup(m => m.Map<UserPrecaution, UserPrecautionDTO>(It.IsAny<UserPrecaution>()))
                .Returns((UserPrecaution src) => new UserPrecautionDTO() { Id = src.Id });

            //Act
            var result = await PrecautionService.GetUserPrecautionAsync(1);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UserPrecautionDTO>(result);
        }
        [Test]
        public async Task UsersTableWithoutPrecautionAsync_ReturnsIEnumerableUserTableDTO()
        {
            // Arrange

            adminService.Setup(a => a.GetUsersTableAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .ReturnsAsync(CreateTuple);

            mockRepoWrapper.Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(GetTestUserPrecaution());

            // Act
            var result = await PrecautionService.UsersTableWithoutPrecautionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<UserTableDTO>>(result);
        }

        readonly UserPrecaution nullPrecaution = null;
        readonly UserPrecautionDTO nullPrecautionDTO = null;
        readonly List<UserPrecaution> nulluserPrecautions = null;
        readonly List<UserPrecautionDTO> nulluserPrecautionsDTO = null;

        private string UserId => _uniqueId.GetUniqueId().ToString();

        private UserPrecaution userPrecaution => new UserPrecaution
        {
            Id = 1,
            Precaution = new Precaution
            {
                Id = 1,
                Name = "За силу",
                UserPrecautions = new List<UserPrecaution>() { new UserPrecaution() }
            },
            UserId = UserId,
            Date = DateTime.Now,
            User = new User
            {
                FirstName = "",
                LastName = "",
                FatherName = "",
                PhoneNumber = "",
                Id = UserId,
                ImagePath = "",
                Email = "",
                UserName = "",
                UserDistinctions = new List<UserDistinction>(),
                UserProfile = new UserProfile()
            },
            PrecautionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""
        };

        private UserPrecautionDTO userPrecautionDTO => new UserPrecautionDTO
        {
            Id = 1,
            Precaution = new PrecautionDTO { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new BLL.DTO.City.CityUserDTO
            {
                FirstName = "",
                LastName = "",
                FatherName = "",
                Email = "",
                ID = UserId,
                ImagePath = "",
                PhoneNumber = ""
            },
            PrecautionId = 1,
            Number = 1,
            Reason = "",
            Reporter = ""
        };

        private IEnumerable<UserTableDTO> GetTestUserTableDTO()
        {
            return new List<UserTableDTO>
            {
                new  UserTableDTO
                {
                    User = new ShortUserInformationDTO { ID = UserId }
                },
                new  UserTableDTO
                {
                    User = new ShortUserInformationDTO { ID = UserId }
                }
            }.AsEnumerable();
        }

        private IEnumerable<UserPrecaution> GetTestUserPrecaution()
        {
            return new List<UserPrecaution>
            {
               new  UserPrecaution
               {
                   Precaution = new Precaution{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecaution
               {
                   Precaution = new Precaution{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecaution
               {
                   Precaution = new Precaution{Id = 3, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName =""}
               }
            }.AsEnumerable();
        }

        private IEnumerable<UserPrecautionDTO> GetTestUserPrecautionDTO()
        {
            return new List<UserPrecautionDTO>
            {
               new  UserPrecautionDTO
               {
                   Precaution = new PrecautionDTO{Id = 1, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecautionDTO
               {
                   Precaution = new PrecautionDTO{Id = 2, Name = "За силу"},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecautionDTO
               {
                   Precaution = new PrecautionDTO{Id = 3, Name = "За силу"},
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

