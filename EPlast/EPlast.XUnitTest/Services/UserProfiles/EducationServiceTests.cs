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
using System.Text;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class EducationServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public EducationServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllGroupByPlaceTest()
        {
            _repoWrapper.Setup(r => r.Education.FindAll()).Returns(new List<Education>().AsQueryable());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = service.GetAllGroupByPlace();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public void GetAllGroupBySpecialityTest()
        {
            _repoWrapper.Setup(r => r.Education.FindAll()).Returns(new List<Education>().AsQueryable());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = service.GetAllGroupBySpeciality();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<EducationDTO>>(result);
        }
        [Fact]
        public void GetByIdTest()
        {
            _repoWrapper.Setup(r => r.Education.FindByCondition(It.IsAny<Expression<Func<Education, bool>>>())).Returns(new List<Education>().AsQueryable());

            var service = new EducationService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Education, EducationDTO>(It.IsAny<Education>())).Returns(new EducationDTO());
            // Act
            var result = service.GetById(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<EducationDTO>(result);
        }
    }
}
