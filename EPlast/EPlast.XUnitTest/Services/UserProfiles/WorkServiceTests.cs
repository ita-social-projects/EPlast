using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class WorkServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public WorkServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllGroupByPlaceTest()
        {
            _repoWrapper.Setup(r => r.Work.FindAll()).Returns(new List<Work>().AsQueryable());

            var service = new WorkService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = service.GetAllGroupByPlace();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<WorkDTO>>(result);
        }
        [Fact]
        public void GetAllGroupByPositionTest()
        {
            _repoWrapper.Setup(r => r.Work.FindAll()).Returns(new List<Work>().AsQueryable());

            var service = new WorkService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = service.GetAllGroupByPosition();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<WorkDTO>>(result);
        }
        [Fact]
        public void GetByIdTest()
        {
            _repoWrapper.Setup(r => r.Work.FindByCondition(It.IsAny<Expression<Func<Work, bool>>>())).Returns(new List<Work>().AsQueryable());

            var service = new WorkService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Work, WorkDTO>(It.IsAny<Work>())).Returns(new WorkDTO());
            // Act
            var result = service.GetById(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<WorkDTO>(result);
        }
    }
}
