using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Services;
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

namespace EPlast.Tests.Services.Precautions
{
    class PrecautionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private Mock<UserManager<User>> userManager;
        private PrecautionService PrecautionService;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            PrecautionService = new PrecautionService(mockMapper.Object, mockRepoWrapper.Object, userManager.Object);
        }

        [Test]
        public async Task GetPrecautionAsync_ById_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Precaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
                .ReturnsAsync(new Precaution());
            mockMapper.Setup(m => m.Map<PrecautionDTO>(It.IsAny<Precaution>()))
                .Returns(new PrecautionDTO());

            //Act
            var result = await PrecautionService.GetPrecautionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetPrecautionAsync_ById_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Precaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
                .ReturnsAsync(new Precaution());
            mockMapper.Setup(m => m.Map<PrecautionDTO>(It.IsAny<Precaution>()))
                .Returns(new PrecautionDTO());

            //Act
            var result = await PrecautionService.GetPrecautionAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<PrecautionDTO>(result);
        }

        [Test]
        public async Task GetPrecautionAsync_ById_Null()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Precaution.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
                .ReturnsAsync(nullPrecaution);
            mockMapper.Setup(m => m.Map<PrecautionDTO>(It.IsAny<Precaution>()))
                .Returns(nullPrecautionDTO);

            //Act
            var result = await PrecautionService.GetPrecautionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllPrecautionAsync_NotNull()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Precaution.GetAllAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>())).ReturnsAsync(GetTestPlastPrecaution());
            mockMapper.Setup(m => m.Map<IEnumerable<PrecautionDTO>>(It.IsAny<IEnumerable<Precaution>>()))
                .Returns(GetTestPlastPrecautionDTO());

            //Act
            var result = await PrecautionService.GetAllPrecautionAsync();

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllPrecautionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Precaution.GetAllAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>())).ReturnsAsync(GetTestPlastPrecaution());
            mockMapper.Setup(m => m.Map<IEnumerable<PrecautionDTO>>(It.IsAny<IEnumerable<Precaution>>()))
                .Returns(GetTestPlastPrecautionDTO());

            //Act
            var result = await PrecautionService.GetAllPrecautionAsync();

            //Assert
            Assert.IsInstanceOf<IEnumerable<PrecautionDTO>>(result);
        }

        [Test]
        public async Task GetAllPrecautionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Precaution.GetAllAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                    It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>())).ReturnsAsync(nullListPrecaution);
            mockMapper.Setup(m => m.Map<IEnumerable<PrecautionDTO>>(It.IsAny<IEnumerable<Precaution>>()))
                .Returns(nullListPrecautionDTO);

            //Act
            var result = await PrecautionService.GetAllPrecautionAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetUsersPrecautionsForTable_ReturnsUserDistinctionsTableObject()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.UserPrecaution.GetUsersPrecautions(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<UserPrecautionsTableObject>());

            //Act
            var result = PrecautionService.GetUsersPrecautionsForTable(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<UserPrecautionsTableObject>>(result);
        }

        [Test]
        public void DeletePrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                   It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
               .ReturnsAsync(Precaution);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.DeletePrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeletePrecautionAsync_IfAdmin_ThrowsArgumentNullException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                   It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
               .ReturnsAsync(nullPrecaution);

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(ArgumentNullException),
                async () => { await PrecautionService.DeletePrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Value cannot be null. (Parameter 'Precaution with 0 not found')", exception.Message);
        }

        [Test]
        public void DeletePrecautionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                   It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
               .ReturnsAsync(new Precaution());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await PrecautionService.DeletePrecautionAsync(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangePrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                   It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
               .ReturnsAsync(Precaution);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.ChangePrecautionAsync(PrecautionDTO, It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangePrecautionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.GetFirstAsync(It.IsAny<Expression<Func<Precaution, bool>>>(),
                   It.IsAny<Func<IQueryable<Precaution>, IIncludableQueryable<Precaution, object>>>()))
               .ReturnsAsync(new Precaution());

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await PrecautionService.ChangePrecautionAsync(PrecautionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddPrecautionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Precaution.CreateAsync(It.IsAny<Precaution>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await PrecautionService.AddPrecautionAsync(It.IsAny<PrecautionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddPrecautionAsync_IfAdmin_WorksCorrectly()
        {
            mockRepoWrapper
               .Setup(x => x.Precaution.CreateAsync(It.IsAny<Precaution>()));

            Assert.DoesNotThrowAsync(async () => { await PrecautionService.AddPrecautionAsync(new PrecautionDTO(), new User()); });
        }

        readonly Precaution nullPrecaution = null;
        readonly PrecautionDTO nullPrecautionDTO = null;
        readonly List<PrecautionDTO> nullListPrecautionDTO = null;
        readonly List<Precaution> nullListPrecaution = null;
        readonly PrecautionDTO PrecautionDTO = new PrecautionDTO { Id = 1, Name = "За силу" };
        readonly Precaution Precaution = new Precaution { Id = 1, Name = "За силу" };

        private IEnumerable<Precaution> GetTestPlastPrecaution()
        {
            return new List<Precaution>
            {
                new Precaution{Id = 1, Name = "За силу"},
                new Precaution{Id = 2, Name = "За волю"},
                new Precaution{Id = 3, Name = "За народ"}
            }.AsEnumerable();
        }

        private IEnumerable<PrecautionDTO> GetTestPlastPrecautionDTO()
        {
            return new List<PrecautionDTO>
            {
                new PrecautionDTO{Id = 1, Name = "За силу"},
                new PrecautionDTO{Id = 2, Name = "За волю"},
                new PrecautionDTO{Id = 3, Name = "За народ"}
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
