using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EPlast.BLL;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                Setup(x => x.Map<EducatorsStaffTypesDTO, EducatorsStaffTypes>
                (It.IsAny<EducatorsStaffTypesDTO>())).
                Returns(new EducatorsStaffTypes());
            _repositoryWrapper.
                Setup(x => x.KVTypes.CreateAsync(It.IsAny<EducatorsStaffTypes>()));
            _repositoryWrapper.Setup(x => x.SaveAsync());
            _mapper.
                Setup(x => x.Map<EducatorsStaffTypes, EducatorsStaffTypesDTO>
                (It.IsAny<EducatorsStaffTypes>())).
                Returns(new EducatorsStaffTypesDTO());

            // Act
            var result = await _educatorsStaffTypesService.
                CreateKVType(It.IsAny<EducatorsStaffTypesDTO>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffTypesDTO>(result);

        }

        [Test]
        public async Task GetAllKVTypesAsync_Test()
        {
            _repositoryWrapper.
                Setup(r => r.KVTypes.GetAllAsync(null, null)).
                ReturnsAsync(new List<EducatorsStaffTypes>().AsQueryable());
            _mapper.
                Setup(x => x.Map<IEnumerable<EducatorsStaffTypes>, IEnumerable<EducatorsStaffTypesDTO>>
                (It.IsAny<IEnumerable<EducatorsStaffTypes>>())).
                Returns(GetTestEducatorsStaffTypesDTO());
            // Act
            var result = await _educatorsStaffTypesService.GetAllKVTypesAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffTypesDTO>>(result);
        }

        [Test]
        public async Task GetKadrasWithSuchType_Test()
        {
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(null, null)).
                ReturnsAsync(new List<EducatorsStaff>().AsQueryable());
            _mapper.
                Setup(x => x.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>
                (It.IsAny<IEnumerable<EducatorsStaff>>())).
                Returns(GetTestEducatorsStaffDTO());
            // Act
            var result = await _educatorsStaffTypesService.GetKadrasWithSuchType(It.IsAny<EducatorsStaffTypesDTO>());
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffDTO>>(result);
        }

        [Test]
        public async Task GetKVsTypeByIdAsync()
        {
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync
                (It.IsAny<Expression<Func<EducatorsStaff, bool>>>(), null)).
                ReturnsAsync(new EducatorsStaff());
            _mapper.
                Setup(x => x.Map<EducatorsStaff, EducatorsStaffDTO>(It.IsAny<EducatorsStaff>())).
                Returns(new EducatorsStaffDTO());
            _repositoryWrapper.
                Setup(r => r.KVTypes.GetFirstOrDefaultAsync
                (It.IsAny<Expression<Func<EducatorsStaffTypes, bool>>>(), null)).
                ReturnsAsync(new EducatorsStaffTypes());
            _mapper.
                Setup(x => x.Map<EducatorsStaffTypes, EducatorsStaffTypesDTO>(It.IsAny<EducatorsStaffTypes>())).
                Returns(new EducatorsStaffTypesDTO());
            // Act
            var result = await _educatorsStaffTypesService.GetKVsTypeByIdAsync(It.IsAny<int>());
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffTypesDTO>(result);
        }

        private IEnumerable<EducatorsStaffTypesDTO> GetTestEducatorsStaffTypesDTO()
        {
            return new List<EducatorsStaffTypesDTO>
            {
                new EducatorsStaffTypesDTO{ID = 1, Name = "scdcsc"},
                new EducatorsStaffTypesDTO{ID = 2, Name = "scascac"},
                new EducatorsStaffTypesDTO{ID = 3, Name = "fvdsvds"}
            }.AsEnumerable();
        }

        private IEnumerable<EducatorsStaffDTO> GetTestEducatorsStaffDTO()
        {
            return new List<EducatorsStaffDTO>
            {
                new EducatorsStaffDTO{ID = 1},
                new EducatorsStaffDTO{ID = 2},
                new EducatorsStaffDTO{ID = 3}
            }.AsEnumerable();
        }
    }
}
