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
    public class DegreeServiceTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public DegreeServiceTest()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllTest()
        {
            _repoWrapper.Setup(r => r.Degree.FindAll()).Returns(new List<Degree>().AsQueryable());

            var service = new DegreeService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Degree, DegreeDTO>(It.IsAny<Degree>())).Returns(new DegreeDTO());
            // Act
            var result = service.GetAll();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<DegreeDTO>>(result);
        }
    }
}
