using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EPlast.BLL;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.EducatorStaff
{
    [TestFixture]
    class EducatorsStaffTypesServiceTests
    {
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mock<IMapper> _mapper;

        private EducatorsStaffTypesService _educatorsStaffTypesService;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();

            _educatorsStaffTypesService = new EducatorsStaffTypesService(
                _repositoryWrapper.Object,
                _mapper.Object);
        }

        [Test]
        public async Task Create_KVType_Test()
        {
            // Arange
            _mapper.
                Setup(x => x.Map<EducatorsStaffTypesDto, EducatorsStaffTypes>
                (It.IsAny<EducatorsStaffTypesDto>())).
                Returns(new EducatorsStaffTypes());
            _repositoryWrapper.
                Setup(x => x.KVTypes.CreateAsync(It.IsAny<EducatorsStaffTypes>()));
            _repositoryWrapper.Setup(x => x.SaveAsync());
            _mapper.
                Setup(x => x.Map<EducatorsStaffTypes, EducatorsStaffTypesDto>
                (It.IsAny<EducatorsStaffTypes>())).
                Returns(new EducatorsStaffTypesDto());

            // Act
            var result = await _educatorsStaffTypesService.
                CreateKVType(It.IsAny<EducatorsStaffTypesDto>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffTypesDto>(result);

        }

        [Test]
        public async Task GetAllKVTypesAsync_Test()
        {
            _repositoryWrapper.
                Setup(r => r.KVTypes.GetAllAsync(null, null)).
                ReturnsAsync(new List<EducatorsStaffTypes>().AsQueryable());
            _mapper.
                Setup(x => x.Map<IEnumerable<EducatorsStaffTypes>, IEnumerable<EducatorsStaffTypesDto>>
                (It.IsAny<IEnumerable<EducatorsStaffTypes>>())).
                Returns(GetTestEducatorsStaffTypesDTO());
            // Act
            var result = await _educatorsStaffTypesService.GetAllKVTypesAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffTypesDto>>(result);
        }

        [Test]
        public async Task GetKadrasWithSuchType_Test()
        {
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(null, null)).
                ReturnsAsync(new List<EducatorsStaff>().AsQueryable());
            _mapper.
                Setup(x => x.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDto>>
                (It.IsAny<IEnumerable<EducatorsStaff>>())).
                Returns(GetTestEducatorsStaffDTO());
            // Act
            var result = await _educatorsStaffTypesService.GetKadrasWithSuchType(It.IsAny<EducatorsStaffTypesDto>());
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffDto>>(result);
        }

        [Test]
        public async Task GetKVsTypeByIdAsync()
        {
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync
                (It.IsAny<Expression<Func<EducatorsStaff, bool>>>(), null)).
                ReturnsAsync(new EducatorsStaff());
            _mapper.
                Setup(x => x.Map<EducatorsStaff, EducatorsStaffDto>(It.IsAny<EducatorsStaff>())).
                Returns(new EducatorsStaffDto());
            _repositoryWrapper.
                Setup(r => r.KVTypes.GetFirstOrDefaultAsync
                (It.IsAny<Expression<Func<EducatorsStaffTypes, bool>>>(), null)).
                ReturnsAsync(new EducatorsStaffTypes());
            _mapper.
                Setup(x => x.Map<EducatorsStaffTypes, EducatorsStaffTypesDto>(It.IsAny<EducatorsStaffTypes>())).
                Returns(new EducatorsStaffTypesDto());
            // Act
            var result = await _educatorsStaffTypesService.GetKVsTypeByIdAsync(It.IsAny<int>());
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffTypesDto>(result);
        }

        private IEnumerable<EducatorsStaffTypesDto> GetTestEducatorsStaffTypesDTO()
        {
            return new List<EducatorsStaffTypesDto>
            {
                new EducatorsStaffTypesDto{ID = 1, Name = "scdcsc"},
                new EducatorsStaffTypesDto{ID = 2, Name = "scascac"},
                new EducatorsStaffTypesDto{ID = 3, Name = "fvdsvds"}
            }.AsEnumerable();
        }

        private IEnumerable<EducatorsStaffDto> GetTestEducatorsStaffDTO()
        {
            return new List<EducatorsStaffDto>
            {
                new EducatorsStaffDto{ID = 1},
                new EducatorsStaffDto{ID = 2},
                new EducatorsStaffDto{ID = 3}
            }.AsEnumerable();
        }
    }
}
