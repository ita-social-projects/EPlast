using AutoMapper;
using EPlast.BLL;
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
    class DistinctionServiceTests
    {
        private Mock<IRepositoryWrapper> mockRepoWrapper;
        private Mock<IMapper> mockMapper;
        private DistinctionService distinctionService;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockRepoWrapper = new Mock<IRepositoryWrapper>();
            distinctionService = new DistinctionService(mockMapper.Object, mockRepoWrapper.Object);
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
        public async Task GetDistinctionAsync_ById_IsWrigthType()
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
            // Arrange
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
            // Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsWrigthType()
        {
            // Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(GetTestPlastDistinction());
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(GetTestPlastDistinctionDTO());
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            // Assert
            Assert.IsInstanceOf<IEnumerable<DistinctionDTO>>(result);
        }

        [Test]
        public async Task GetAllDistinctionAsync_IsEmpty()
        {
            // Arrange
            mockRepoWrapper.Setup(x => x.Distinction.GetAllAsync(It.IsAny<Expression<Func<Distinction, bool>>>(),
                    It.IsAny<Func<IQueryable<Distinction>, IIncludableQueryable<Distinction, object>>>())).ReturnsAsync(nullListDistinction);
            mockMapper.Setup(m => m.Map<IEnumerable<DistinctionDTO>>(It.IsAny<IEnumerable<Distinction>>()))
                .Returns(nullListDistinctionDTO);
            //Act
            var result = await distinctionService.GetAllDistinctionAsync();
            // Assert
            Assert.IsEmpty(result);
        }

        Distinction nullDistinction = null;
        DistinctionDTO nullDistinctionDTO = null;
        List<DistinctionDTO> nullListDistinctionDTO = null;
        List<Distinction> nullListDistinction = null;

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
    }
}
