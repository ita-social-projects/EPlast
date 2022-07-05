using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Services.Admin;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Admin
{
    [TestFixture]
    public class AdminTypeServiceTests
    {
        private AdminTypeService _adminTypeService;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void SetUp()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _adminTypeService = new AdminTypeService(_mockRepoWrapper.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAdminTypeByIdAsync_Valid_GetAdminTypeAsync()
        {
            //Arrange
            int adminTypeId = 1;
            AdminType adminType = new AdminType() { ID = adminTypeId };
            _mockRepoWrapper.Setup(s => s.AdminType.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AdminType, bool>>>(),
                    It.IsAny<Func<IQueryable<AdminType>, IIncludableQueryable<AdminType, object>>>()))
                .ReturnsAsync(adminType);
            _mockMapper
                .Setup(x => x.Map<AdminType, AdminTypeDto>(adminType))
                .Returns(new AdminTypeDto() { ID = adminType.ID });

            //Act
            var result = await _adminTypeService.GetAdminTypeByIdAsync(adminTypeId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<AdminTypeDto>(result);
            Assert.AreEqual(adminTypeId, result.ID);
        }
    }
}
