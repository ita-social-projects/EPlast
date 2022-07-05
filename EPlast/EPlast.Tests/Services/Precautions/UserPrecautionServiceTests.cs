using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Queries.Precaution;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Precautions;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Precautions
{
    class UserPrecautionServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapperMock;
        private Mock<IMapper> _mapperMock;
        private UserPrecautionService _precautionService;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IAdminService> _adminServiceMock;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _repoWrapperMock = new Mock<IRepositoryWrapper>();
            _adminServiceMock = new Mock<IAdminService>();
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mediatorMock = new Mock<IMediator>();
            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            _precautionService = new UserPrecautionService(_mapperMock.Object, _repoWrapperMock.Object, _userManagerMock.Object,
                _adminServiceMock.Object, _mediatorMock.Object);
        }

        [Test]
        public async Task AddUserPrecautionAsync_SuperAdmin_ReturnsTrue()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(false);
            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            //Act
            var result = await _precautionService.AddUserPrecautionAsync(userPrecautionDTO, new User());

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task AddUserPrecautionAsync_SuperAdminLowerUserInPrecaution_ReturnsFalse()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(false);
            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(Roles.LowerRoles);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            //Act
            var result = await _precautionService.AddUserPrecautionAsync(userPrecautionDTO, new User());

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task AddUserPrecautionAsync_BothUsersGoverningBodyAdmins_ReturnsFalse()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(true);
            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            //Act
            var result = await _precautionService.AddUserPrecautionAsync(userPrecautionDTO, new User());

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task AddUserPrecautionAsync_NumberExists_ReturnsFalse()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(true);
            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);
            //Act
            var result = await _precautionService.AddUserPrecautionAsync(userPrecautionDTO, new User());

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task ChangeUserPrecautionAsync_SuperAdmin_ReturnsTrue()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(false);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act
            var result = await _precautionService.ChangeUserPrecautionAsync(userPrecautionDTO, new User());

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task ChangeUserPrecautionAsync_GoverningBodyUserInactivePrecaution_ReturnsFalse()
        {
            //Arrange

            var currentUser = new User();

            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(currentUser, Roles.GoverningBodyAdmin)).ReturnsAsync(true);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act
            var result = await _precautionService.ChangeUserPrecautionAsync(userPrecautionDTO, currentUser);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task DeleteUserPrecautionAsync_SuperAdmin_ReturnsTrue()
        {
            //Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(false);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act
            var result = await _precautionService.DeleteUserPrecautionAsync(1, new User());

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task DeleteUserPrecautionAsync_GoverningBodyUserInactivePrecaution_ReturnsFalse()
        {
            //Arrange
            var currentUser = new User();

            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(currentUser, Roles.GoverningBodyAdmin)).ReturnsAsync(true);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act
            var result = await _precautionService.ChangeUserPrecautionAsync(userPrecautionDTO, currentUser);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task GetUserPrecautionsForTableAsync_SuperAdmin_ReturnsUserPrecautionsTableInfo()
        {
            //Arrange
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetUsersPrecautionsForTableQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateTuple);
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(false);
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);

            //Act
            var result =
                await _precautionService.GetUserPrecautionsForTableAsync(It.IsAny<PrecautionTableSettings>());

            //Assert 
            Assert.IsInstanceOf<UserPrecautionsTableInfo>(result);

        }

        [Test]
        public async Task IsNumberExistAsync_True()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());
            _mapperMock
                .Setup(m => m.Map<UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns(new UserPrecautionDto());

            //Act
            var result = await _precautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_False()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            _mapperMock
                .Setup(m => m.Map<UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns(nullPrecautionDTO);

            //Act
            var result = await _precautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsNumberExistAsync_IsInstanceOf()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(new UserPrecaution());
            _mapperMock
                .Setup(m => m.Map<UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns(new UserPrecautionDto());

            //Act
            var result = await _precautionService.IsNumberExistAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsInstanceOf()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(GetTestUserPrecaution());

            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserPrecautionDto>>(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsNotNull()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersPrecautionAsync_IsEmpty()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nulluserPrecautions);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
                .Returns(nulluserPrecautionsDTO);

            //Act
            var result = await _precautionService.GetAllUsersPrecautionAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsInstanceOf()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsInstanceOf<IEnumerable<UserPrecautionDto>>(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsNotNull()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserPrecautionsOfUserAsync_IsEmpty()
        {
            //Arrange
            _repoWrapperMock
               .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nulluserPrecautions);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
                .Returns(nulluserPrecautionsDTO);

            //Act
            var result = await _precautionService.GetUserPrecautionsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsNull()
        {
            //Arrange
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            _mapperMock
                .Setup(m => m.Map<UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns(nullPrecautionDTO);

            //Act
            var result = await _precautionService.GetUserPrecautionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsNotNull()
        {
            //Arrange
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);
            _mapperMock
                .Setup(m => m.Map<UserPrecaution, UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns((UserPrecaution src) => new UserPrecautionDto() { Id = src.Id });

            //Act
            var result = await _precautionService.GetUserPrecautionAsync(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserPrecautionAsync_IsInstanceOf()
        {
            //Arrange
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(userPrecaution);
            _mapperMock
                .Setup(m => m.Map<UserPrecaution, UserPrecautionDto>(It.IsAny<UserPrecaution>()))
                .Returns((UserPrecaution src) => new UserPrecautionDto() { Id = src.Id });

            //Act
            var result = await _precautionService.GetUserPrecautionAsync(1);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UserPrecautionDto>(result);
        }
        [Test]
        public async Task UsersTableWithoutPrecautionAsync_ReturnsShortUserInformationDTO()
        {
            // Arrange

            _adminServiceMock.Setup(a => a.GetUsersAsync())
                .ReturnsAsync(GetTestShortUserInfo());
            _repoWrapperMock.Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(GetTestUserPrecaution());

            // Act
            var result = await _precautionService.UsersTableWithoutPrecautionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ShortUserInformationDto>>(result);
        }

        [Test]
        public async Task CheckUserPrecautionsType_Test()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDTO>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act 
            var result = await _precautionService.CheckUserPrecautionsType("a84473c3-140b-4cae-ac80-b7cd5759d3b5", "За силу");

            //Assert
            Assert.True(result);
        }

        [Test]
        public async Task CheckUserPrecautionsType_False_Test()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act 
            var result = await _precautionService.CheckUserPrecautionsType("a84473c3-140b-4cae-ac80-b7cd5759d3b5", "За славу");

            //Assert
            Assert.False(result);
        }
        [Test]
        public async Task GetUserActivePrecaution_IsNotNull()
        {
            //Arrange
            _repoWrapperMock
              .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                   It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
               .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetUserActivePrecaution("a84473c3-140b-4cae-ac80-b7cd5759d3b5", "За силу");

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserActivePrecaution_IsNull()
        {
            //Arrange
            _repoWrapperMock
                .Setup(x => x.UserPrecaution.GetAllAsync(It.IsAny<Expression<Func<UserPrecaution, bool>>>(),
                    It.IsAny<Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>>>()))
                .ReturnsAsync(GetTestUserPrecaution());
            _mapperMock
               .Setup(m => m.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDto>>(It.IsAny<IEnumerable<UserPrecaution>>()))
               .Returns(GetTestUserPrecautionDTO());

            //Act
            var result = await _precautionService.GetUserActivePrecaution("a84473c3-140b-4cae-ac80-b7cd5759d3b5", "За славу");

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUsersForPrecautionAsync_AdminAllUsersAvailable_ReturnsListOfAvailableUsers()
        {
            //Arrange
            var suggestedUser = new SuggestedUserDto
            {
                ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                Email = "test@mail.com",
                FirstName = "John",
                LastName = "Brian",
            };
            var assignableRoles = new List<string>
            {
                Roles.KurinHead,
                Roles.CityHead
            };
            var currentUser = new User();

            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.Admin)).ReturnsAsync(true);

            _repoWrapperMock.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(GetTestUsers());
            _mapperMock.Setup(m => m.Map<User, SuggestedUserDto>(It.IsAny<User>())).Returns(suggestedUser);

            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(assignableRoles);
            var expected = GetAvailableSuggestedUsers().ToList();

            //Act
            var result = await _precautionService.GetUsersForPrecautionAsync(currentUser);
            var resultList = result.ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<SuggestedUserDto>>(result);
            Assert.AreEqual(expected.Count, resultList.Count);
            Assert.AreEqual(expected[0].IsAvailable, resultList[0].IsAvailable);
            Assert.AreEqual(expected[1].IsAvailable, resultList[1].IsAvailable);
        }

        [Test]
        public async Task GetUsersForPrecautionAsync_AdminUsersHaveLowerRole_ReturnsListOfUnavailableUsers()
        {
            //Arrange
            var suggestedUser = new SuggestedUserDto
            {
                ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                Email = "test@mail.com",
                FirstName = "John",
                LastName = "Brian",
            };

            var currentUser = new User();

            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.Admin)).ReturnsAsync(true);

            _repoWrapperMock.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(GetTestUsers());
            _mapperMock.Setup(m => m.Map<User, SuggestedUserDto>(It.IsAny<User>())).Returns(suggestedUser);

            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(Roles.LowerRoles);
            var expected = GetUnavailableSuggestedUsers().ToList();

            //Act
            var result = await _precautionService.GetUsersForPrecautionAsync(currentUser);
            var resultList = result.ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<SuggestedUserDto>>(result);
            Assert.AreEqual(expected.Count, resultList.Count);
            Assert.AreEqual(expected[0].IsAvailable, resultList[0].IsAvailable);
            Assert.AreEqual(expected[1].IsAvailable, resultList[1].IsAvailable);
        }

        [Test]
        public async Task GetUsersForPrecautionAsync_GoverningBodyAdminAllUsersAvailable_ReturnsListOfAvailableUsers()
        {
            //Arrange
            var suggestedUser = new SuggestedUserDto
            {
                ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                Email = "test@mail.com",
                FirstName = "John",
                LastName = "Brian",
            };
            var assignableRoles = new List<string>
            {
                Roles.KurinHead,
                Roles.CityHead
            };
            var currentUser = new User();

            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(true);

            _repoWrapperMock.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(GetTestUsers());
            _mapperMock.Setup(m => m.Map<User, SuggestedUserDto>(It.IsAny<User>())).Returns(suggestedUser);

            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(assignableRoles);
            var expected = GetAvailableSuggestedUsers().ToList();

            //Act
            var result = await _precautionService.GetUsersForPrecautionAsync(currentUser);
            var resultList = result.ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<SuggestedUserDto>>(result);
            Assert.AreEqual(expected.Count, resultList.Count);
            Assert.AreEqual(expected[0].IsAvailable, resultList[0].IsAvailable);
            Assert.AreEqual(expected[1].IsAvailable, resultList[1].IsAvailable);
        }

        [Test]
        public async Task GetUsersForPrecautionAsync_GoverningBodyAdminUsersHaveSameRole_ReturnsListOfUnavailableUsers()
        {
            //Arrange
            var suggestedUser = new SuggestedUserDto
            {
                ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                Email = "test@mail.com",
                FirstName = "John",
                LastName = "Brian",
            };
            var unassignableRoles = new List<string>
            {
                Roles.GoverningBodyAdmin
            };
            var currentUser = new User();

            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(true);

            _repoWrapperMock.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(GetTestUsers());
            _mapperMock.Setup(m => m.Map<User, SuggestedUserDto>(It.IsAny<User>())).Returns(suggestedUser);

            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(unassignableRoles);
            var expected = GetUnavailableSuggestedUsers().ToList();

            //Act
            var result = await _precautionService.GetUsersForPrecautionAsync(currentUser);
            var resultList = result.ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<SuggestedUserDto>>(result);
            Assert.AreEqual(expected.Count, resultList.Count);
            Assert.AreEqual(expected[0].IsAvailable, resultList[0].IsAvailable);
            Assert.AreEqual(expected[1].IsAvailable, resultList[1].IsAvailable);
        }

        [Test]
        public async Task GetUsersForPrecautionAsync_GoverningBodyAdminUsersHaveLowerRole_ReturnsListOfUnavailableUsers()
        {
            //Arrange
            var suggestedUser = new SuggestedUserDto
            {
                ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                Email = "test@mail.com",
                FirstName = "John",
                LastName = "Brian",
            };

            var currentUser = new User();

            _userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), Roles.GoverningBodyAdmin)).ReturnsAsync(true);

            _repoWrapperMock.Setup(x => x.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(GetTestUsers());
            _mapperMock.Setup(m => m.Map<User, SuggestedUserDto>(It.IsAny<User>())).Returns(suggestedUser);

            _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(Roles.LowerRoles);
            var expected = GetUnavailableSuggestedUsers().ToList();

            //Act
            var result = await _precautionService.GetUsersForPrecautionAsync(currentUser);
            var resultList = result.ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<SuggestedUserDto>>(result);
            Assert.AreEqual(expected.Count, resultList.Count);
            Assert.AreEqual(expected[0].IsAvailable, resultList[0].IsAvailable);
            Assert.AreEqual(expected[1].IsAvailable, resultList[1].IsAvailable);
        }

        readonly UserPrecaution nullPrecaution = null;
        readonly UserPrecautionDto nullPrecautionDTO = null;
        readonly List<UserPrecaution> nulluserPrecautions = null;
        readonly List<UserPrecautionDto> nulluserPrecautionsDTO = null;

        private string UserId => Guid.NewGuid().ToString();

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

        private UserPrecautionDto userPrecautionDTO => new UserPrecautionDto
        {
            Id = 1,
            Precaution = new PrecautionDto { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new PrecautionUserDto()
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

        private UserPrecautionDto userPrecautionDTO2 => new UserPrecautionDto
        {
            Id = 1,
            Precaution = new PrecautionDto { Id = 1, Name = "За все" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new PrecautionUserDto()
            {
                FirstName = "",
                LastName = "",
                FatherName = "",
                Email = "",
                ID = UserId,
                ImagePath = "",
                PhoneNumber = ""
            },
            PrecautionId = 2,
            Number = 1,
            Reason = "",
            Reporter = ""
        };

        private IEnumerable<ShortUserInformationDto> GetTestShortUserInfo()
        {
            return new List<ShortUserInformationDto>
            {
                new ShortUserInformationDTO { ID = UserId },
                new ShortUserInformationDTO { ID = UserId }

            }.AsEnumerable();
        }

        private IEnumerable<UserPrecaution> GetTestUserPrecaution()
        {
            return new List<UserPrecaution>
            {
               new  UserPrecaution
               {
                   Precaution = new Precaution{Id = 1, Name = "За силу"},
                   UserId = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
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

        private IEnumerable<UserPrecautionDto> GetTestUserPrecautionDTO()
        {
            return new List<UserPrecautionDto>
            {
               new UserPrecautionDTO
               {
                   Precaution = new PrecautionDTO{Id = 1, Name = "За силу", MonthsPeriod = 6},
                   UserId = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                   Date = DateTime.Now,
                   User = new PrecautionUserDto { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecautionDto
               {
                   Precaution = new PrecautionDTO{Id = 2, Name = "За силу", MonthsPeriod = 12},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new PrecautionUserDto { FirstName = "", LastName = "", FatherName =""}
               },
               new  UserPrecautionDto
               {
                   Precaution = new PrecautionDTO{Id = 3, Name = "За силу", MonthsPeriod = 9},
                   UserId = UserId,
                   Date = DateTime.Now,
                   User = new PrecautionUserDto { FirstName = "", LastName = "", FatherName =""}
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

        private IEnumerable<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                },
                new User
                {
                    Id = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                }
            }.AsEnumerable();
        }

        private IEnumerable<SuggestedUserDto> GetAvailableSuggestedUsers()
        {
            return new List<SuggestedUserDto>
            {
                new SuggestedUserDto
                {
                    ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                    IsAvailable = true
                },
                new SuggestedUserDto
                {
                    ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                    IsAvailable = true
                }
            }.AsEnumerable();
        }
        private IEnumerable<SuggestedUserDto> GetUnavailableSuggestedUsers()
        {
            return new List<SuggestedUserDto>
            {
                new SuggestedUserDto
                {
                    ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                    IsAvailable = false
                },
                new SuggestedUserDto
                {
                    ID = "a84473c3-140b-4cae-ac80-b7cd5759d3b5",
                    Email = "test@mail.com",
                    FirstName = "John",
                    LastName = "Brian",
                    IsAvailable = false
                }
            }.AsEnumerable();
        }

        private Tuple<IEnumerable<UserPrecautionsTableObject>, int> CreateTuple => new Tuple<IEnumerable<UserPrecautionsTableObject>, int>(GetUsersPrecautionByPage(), GetFakeUserPrecautionNumber());

        private List<UserPrecautionsTableObject> GetUsersPrecautionByPage()
        {
            return new List<UserPrecautionsTableObject>()
            {
                new UserPrecautionsTableObject()
                {
                    Number = 34,
                }
            };
        }

        private int GetFakeUserPrecautionNumber()
        {
            return 100;
        }
    }
}

