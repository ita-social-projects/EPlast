using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.Admin;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.ClubTests
{
    public class AdminTypeServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IMapper> _mapper;
        private readonly IAdminTypeService _adminTypeService;

        public AdminTypeServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _adminTypeService = new AdminTypeService(_repoWrapper.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetAdminTypeByNameAsyncTest()
        {
            //Arrange
            _repoWrapper.Setup(s => s.AdminType.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(new AdminType());
            _mapper
                .Setup(s => s.Map<AdminType, AdminTypeDTO>(It.IsAny<AdminType>()))
                .Returns(new AdminTypeDTO());

            //Act
            var result = await _adminTypeService.GetAdminTypeByNameAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AdminTypeDTO>(result);
        }

        [Fact]
        public async Task CreateByNameAsyncTest()
        {
            //Arrange
            _repoWrapper.Setup(s => s.AdminType.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(new AdminType());
            _mapper
                .Setup(s => s.Map<AdminType, AdminTypeDTO>(It.IsAny<AdminType>()))
                .Returns(new AdminTypeDTO());

            //Act
            var result = await _adminTypeService.CreateByNameAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AdminTypeDTO>(result);
            _repoWrapper.Verify(r => r.AdminType.CreateAsync(It.IsAny<AdminType>()));
            _repoWrapper.Verify(r => r.SaveAsync());
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            //Arrange
            _repoWrapper.Setup(s => s.AdminType.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(new AdminType());
            _mapper
                .Setup(s => s.Map<AdminType, AdminTypeDTO>(It.IsAny<AdminType>()))
                .Returns(new AdminTypeDTO());

            //Act
            var result = await _adminTypeService.CreateAsync(new AdminTypeDTO());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AdminTypeDTO>(result);
            _repoWrapper.Verify(r => r.AdminType.CreateAsync(It.IsAny<AdminType>()));
            _repoWrapper.Verify(r => r.SaveAsync());
        }
    }
}