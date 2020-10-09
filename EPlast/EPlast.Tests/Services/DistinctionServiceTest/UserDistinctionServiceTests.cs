using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Services.Distinctions;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
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

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            distinctionService = new UserDistinctionService(mockMapper.Object, mockRepoWrapper.Object);
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
            //Arrange
            Assert.IsNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_True()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());
            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());
            //Arrange
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task IsNumberExistAsync_False()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nullDistinction);
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(nullDistinctionDTO);
            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());
            //Arrange
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsNumberExistAsync_IsWrigthType()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(new UserDistinction());
            mockMapper
                .Setup(m => m.Map<UserDistinctionDTO>(It.IsAny<UserDistinction>()))
                .Returns(new UserDistinctionDTO());
            //Act
            var result = await distinctionService.IsNumberExistAsync(It.IsAny<int>());
            //Arrange
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsWrigthType()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());
            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();
            //Arrange
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsNotNull()
        {
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());
            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();
            //Arrange
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllUsersDistinctionAsync_IsEmpty()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);
            //Act
            var result = await distinctionService.GetAllUsersDistinctionAsync();
            //Arrange
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsWrigthType()
        {
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());
            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny <string>());
            //Arrange
            Assert.IsInstanceOf<IEnumerable<UserDistinctionDTO>>(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsNotNull()
        {
            mockRepoWrapper
              .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                   It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
               .ReturnsAsync(GetTestUserDistinction());
            mockMapper
               .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
               .Returns(GetTestUserDistinctionDTO());
            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());
            //Arrange
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserDistinctionsOfUserAsync_IsEmpty()
        {
            mockRepoWrapper
               .Setup(x => x.UserDistinction.GetAllAsync(It.IsAny<Expression<Func<UserDistinction, bool>>>(),
                    It.IsAny<Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>>>()))
                .ReturnsAsync(nulluserDistinctions);
            mockMapper
                .Setup(m => m.Map<IEnumerable<UserDistinctionDTO>>(It.IsAny<IEnumerable<UserDistinction>>()))
                .Returns(nulluserDistinctionsDTO);
            //Act
            var result = await distinctionService.GetUserDistinctionsOfUserAsync(It.IsAny<string>());
            //Arrange
            Assert.IsEmpty(result);
        }

        UserDistinction nullDistinction = null;
        UserDistinctionDTO nullDistinctionDTO = null;
        List<UserDistinction> nulluserDistinctions = null;
        List<UserDistinctionDTO> nulluserDistinctionsDTO = null;
        
        private string UserId => Guid.NewGuid().ToString();

        private UserDistinction userDistinction => new UserDistinction
        {
            Distinction = new Distinction { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new DataAccess.Entities.User { FirstName = "", LastName = "", FatherName = "" }
        };

        private UserDistinctionDTO userDistinctionDTO => new UserDistinctionDTO
        {
            Distinction = new DistinctionDTO { Id = 1, Name = "За силу" },
            UserId = UserId,
            Date = DateTime.Now,
            User = new BLL.DTO.City.CityUserDTO { FirstName = "", LastName = "", FatherName = "" }
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
    }
}
