using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class NationalityServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public NationalityServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllTest()
        {
            _repoWrapper.Setup(r => r.Nationality.FindAll()).Returns(new List<Nationality>().AsQueryable());

            var service = new NationalityService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Nationality, NationalityDTO>(It.IsAny<Nationality>())).Returns(new NationalityDTO());
            // Act
            var result = service.GetAll();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<NationalityDTO>>(result);
        }
    }
}
