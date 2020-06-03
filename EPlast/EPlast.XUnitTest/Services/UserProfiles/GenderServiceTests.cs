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
    public class GenderServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public GenderServiceTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllTest()
        {
            _repoWrapper.Setup(r => r.Gender.FindAll()).Returns(new List<Gender>().AsQueryable());

            var service = new GenderService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Gender, GenderDTO>(It.IsAny<Gender>())).Returns(new GenderDTO());
            // Act
            var result = service.GetAll();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<GenderDTO>>(result);
        }
    }
}
