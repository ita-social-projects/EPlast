using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.UserProfiles
{
    class UserPersonalDataServiceTests
    {
        private  Mock<IRepositoryWrapper> _repoWrapper;
        private  Mock<IMapper> _mapper;

        private IUserPersonalDataService _userPersonalDataService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _userPersonalDataService = new UserPersonalDataService(
                _repoWrapper.Object,
                _mapper.Object);
        }

        [Test]
        public async Task GetAllUpuDegreesAsync_Valid()
        {
            //Arrange
            _repoWrapper.Setup(r => r.UpuDegree.GetAllAsync(It.IsAny<Expression<Func<UpuDegree, bool>>>(),
                It.IsAny<Func<IQueryable<UpuDegree>,
                    IIncludableQueryable<UpuDegree, object>>>())).ReturnsAsync(new List<UpuDegree>().AsQueryable());
            _mapper.Setup(x => x.Map<UpuDegree, UpuDegreeDTO>(It.IsAny<UpuDegree>())).Returns(new UpuDegreeDTO());

            // Act
            var result = await _userPersonalDataService.GetAllUpuDegreesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<UpuDegreeDTO>>(result);
        }
    }
}
