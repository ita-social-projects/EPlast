using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Services.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EPlast.XUnitTest.Services.UserProfiles
{
    public class ReligionServiceTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public ReligionServiceTest()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetAllTest()
        {
            _repoWrapper.Setup(r => r.Religion.FindAll()).Returns(new List<Religion>().AsQueryable());

            var service = new ReligionService(_repoWrapper.Object, _mapper.Object);
            _mapper.Setup(x => x.Map<Religion, ReligionDTO>(It.IsAny<Religion>())).Returns(new ReligionDTO());
            // Act
            var result = service.GetAll();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ReligionDTO>>(result);
        }
    }
}
