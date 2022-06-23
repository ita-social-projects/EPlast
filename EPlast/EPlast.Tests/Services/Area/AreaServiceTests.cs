using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Services;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services
{
    [TestFixture]
    public class AreaServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();

            _repoWrapper.Setup(r => r.AreaRepository)
                .Returns(It.IsAny<IAreaRepository>());

            _repoWrapper.Setup(r => r.AreaRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<Area, bool>>>(),
                    It.IsAny<Func<IQueryable<Area>, IIncludableQueryable<Area, object>>>())
                ).ReturnsAsync(Areas);

            _mapper = new Mock<IMapper>();

            _mapper.Setup(m => m.Map<AreaDTO>(It.IsAny<object>()))
                .Returns(AreaDTO);

            _mapper.Setup(m => m.Map<IEnumerable<AreaDTO>>(It.IsAny<object>()))
                .Returns(AreasDTOs);
        }

        [Test]
        public async Task GetAll_ReturtnAreasDTOs()
        {
            // Arrange
            AreaService areaService = CreateAreaService();


            // Act
            var result = await areaService.GetAllAsync();

            // Assert
            Assert.AreEqual(Areas.Select(a => a.Id), result.Select(a => a.Id));
        }

        [Test]
        public async Task GetOneById_Valid_ReturtnAreaDTO()
        {
            // Arrange
            AreaService areaService = CreateAreaService();

            _repoWrapper.Setup(r => r.AreaRepository
                .GetFirstAsync(
                    It.IsAny<Expression<Func<Area, bool>>>(),
                    It.IsAny<Func<IQueryable<Area>, IIncludableQueryable<Area, object>>>())
                ).ReturnsAsync(Area);

            // Act
            var result = await areaService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.AreEqual(Area.Id, result.Id);
        }

        [Test]
        public void GetOneById_InValid_ThrowNullReferenseException()
        {
            // Arrange
            AreaService areaService = CreateAreaService();

            _repoWrapper.Setup(r => r.AreaRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Area, bool>>>(),
                    It.IsAny<Func<IQueryable<Area>, IIncludableQueryable<Area, object>>>())
                ).Returns(Task.FromResult<Area>(null));

           
            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
               // Act
               await areaService.GetByIdAsync(It.IsAny<int>())
            );
        }

        private IEnumerable<Area> Areas => new List<Area>
        {
            new Area { Id = 1, Name = "Чернівецька" },
            new Area { Id = 2, Name = "Львівська" },
            new Area { Id = 3, Name = "Чернігівська" },
        };

        private IEnumerable<AreaDTO> AreasDTOs => Areas.Select(a => new AreaDTO { Id = a.Id, Name = a.Name });

        private Area Area => Areas.First();
        private AreaDTO AreaDTO => AreasDTOs.First();

        private AreaService CreateAreaService()
        {
            return new AreaService(_repoWrapper.Object, _mapper.Object);
        }
    }
}
