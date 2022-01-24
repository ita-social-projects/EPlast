using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.Distinction;
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
    class DistinctionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private Mock<UserManager<User>> userManager;
        private DistinctionService distinctionService;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRoles());
            distinctionService = new DistinctionService(mockMapper.Object, mockRepoWrapper.Object, userManager.Object);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_IsNotNull()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(new Distinction());
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(new DistinctionDTO());

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(new Distinction());
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(new DistinctionDTO());

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsInstanceOf<DistinctionDTO>(result);
        }

        [Test]
        public async Task GetDistinctionAsync_ById_Null()
        {
            //Arrange
            mockRepoWrapper
                .Setup(x => x.Distinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper.Setup(m => m.Map<DistinctionDTO>(It.IsAny<Distinction>()))
                .Returns(nullDistinctionDTO);

            //Act
            var result = await distinctionService.GetDistinctionAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_NotNull()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsInstanceOf()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsInstanceOf<IEnumerable<DistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsEmpty()
        {
            //Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(nullListDistinction);
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(nullListDistinctionDTO);
            
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            
            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByNumberAscending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "number", "ascend" };
            TestDTS.SearchedData = "2021";

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByNumberDescending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "number", "descend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }

        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByDistinctionNameDescending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "distinctionName", "descend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByDistinctionNameAscending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "distinctionName", "ascend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByUserNameAscending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "userName", "ascend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByUserNameDescending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "userName", "descend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByDateDescending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "date", "descend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }
        [Test]
        public void GetUsersDistinctions_ReturnsUserDistinctionsTableObjectASyncSortingByDateAscending()
        {
            //Arrange
            DistictionTableSettings TestDTS = new DistictionTableSettings();
            TestDTS.SortByOrder = new List<string> { "date", "ascend" };

            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetRangeAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                It.IsAny<Expression<Func<UserDistinction, UserDistinction>>>(), It.IsAny<Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>>>(),
                It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(CreateTuple);

            //Act
            var result = distinctionService.GetUsersDistinctionsForTableAsync(TestDTS).Result;

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<UserDistinctionsTableObject>, int>>(result);
        }

        [Test]
        public void DeleteDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(distinction);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void DeleteDistinctionAsync_IfAdmin_ThrowsArgumentNullException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(nullDistinction);

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(ArgumentNullException),
                async () => { await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
            Assert.AreEqual("Value cannot be null. (Parameter 'Distinction with 0 not found')", exception.Message);
        }

        [Test]
        public void DeleteDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());

            //Assert
            Assert.DoesNotThrowAsync(async () => { await distinctionService.DeleteDistinctionAsync(It.IsAny<int>(), It.IsAny<User>()); });
        }

        [Test]
        public void ChangeDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(distinction);

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.ChangeDistinctionAsync(distinctionDTO, It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void ChangeDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.GetFirstAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                   It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>()))
               .ReturnsAsync(new Distinction());

            //Act

            //Assert
            Assert.DoesNotThrowAsync(async () => { await distinctionService.ChangeDistinctionAsync(distinctionDTO, It.IsAny<User>()); });
        }

        [Test]
        public void AddDistinctionAsync_IfNotAdmin_ThrowsUnauthorizedAccessException()
        {
            //Arrange
            mockRepoWrapper
               .Setup(x => x.Distinction.CreateAsync(It.IsAny<Distinction>()));

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithoutAdmin());

            //Assert
            Exception exception = Assert.ThrowsAsync(typeof(UnauthorizedAccessException),
                async () => { await distinctionService.AddDistinctionAsync(It.IsAny<DistinctionDTO>(), It.IsAny<User>()); });
            Assert.AreEqual("Attempted to perform an unauthorized operation.", exception.Message);
        }

        [Test]
        public void AddDistinctionAsync_IfAdmin_WorksCorrectly()
        {
            mockRepoWrapper
               .Setup(x => x.Distinction.CreateAsync(It.IsAny<Distinction>()));

            Assert.DoesNotThrowAsync(async () => { await distinctionService.AddDistinctionAsync( new DistinctionDTO(), new User()); });
        }

        readonly Distinction nullDistinction = null;
        readonly DistinctionDTO nullDistinctionDTO = null;
        readonly List<DistinctionDTO> nullListDistinctionDTO = null;
        readonly List<Distinction> nullListDistinction = null;
        readonly DistinctionDTO distinctionDTO = new DistinctionDTO { Id = 1, Name = "За силу" };
        readonly Distinction distinction = new Distinction { Id = 1, Name = "За силу" };

        private IEnumerable<Distinction> GetTestPlastDistinction()
        {
            return new List<Distinction>
            {
                new Distinction{Id = 1, Name = "За силу"},
                new Distinction{Id = 2, Name = "За волю"},
                new Distinction{Id = 3, Name = "За народ"}
            }.AsEnumerable();
        }

        private IEnumerable<DistinctionDTO> GetTestPlastDistinctionDTO()
        {
            return new List<DistinctionDTO>
            {
                new DistinctionDTO{Id = 1, Name = "За силу"},
                new DistinctionDTO{Id = 2, Name = "За волю"},
                new DistinctionDTO{Id = 3, Name = "За народ"}
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
        private List<UserDistinction> GetUsersDistinctionByPage()
        {
            return new List<UserDistinction>()
            {
                new UserDistinction()
                {
                    Number = 123,
                }
            };
        }

        private Tuple<IEnumerable<UserDistinction>, int> CreateTuple => new Tuple<IEnumerable<UserDistinction>, int>(GetUsersDistinctionByPage(), 100);
    }
}
