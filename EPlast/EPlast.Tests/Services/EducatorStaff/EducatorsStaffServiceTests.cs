using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
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
    class EducatorsStaffServiceTests
    {
        private Mock<IRepositoryWrapper> _repositoryWrapper;
        private Mock<IMapper> _mapper;

        private EducatorsStaffService _educatorsStaffService;

        [SetUp]
        public void SetUp()
        {
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();

            _educatorsStaffService = new EducatorsStaffService(
                _repositoryWrapper.Object,
                _mapper.Object);
        }

        [Test]
        public async Task CreateKadra_Test()
        {
            // Arange
            _mapper.
                Setup(x => x.Map<EducatorsStaffDTO, EducatorsStaff>
                (It.IsAny<EducatorsStaffDTO>())).
                Returns(new EducatorsStaff());
            _repositoryWrapper.
                Setup(x => x.KVs.CreateAsync(It.IsAny<EducatorsStaff>()));
            _repositoryWrapper.Setup(x => x.SaveAsync());
            _mapper.
                Setup(x => x.Map<EducatorsStaff, EducatorsStaffDTO>
                (It.IsAny<EducatorsStaff>())).
                Returns(new EducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffService.
                CreateKadra(It.IsAny<EducatorsStaffDTO>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffDTO>(result);
        }

        [Test]
        public async Task DeleteKadra_KadraDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _repositoryWrapper.
                Setup(rw => rw.KVs.GetFirstAsync(
                It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());
            _repositoryWrapper.
                Setup(r => r.KVs.Delete(It.IsAny<EducatorsStaff>()));

            //Act
            await _educatorsStaffService.DeleteKadra(It.IsAny<int>());

            // Assert
            _repositoryWrapper.VerifyAll();
        }

        [Test]
        public async Task GetAllKVsAsync_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>()));
            _mapper.
                Setup(m => m.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(It.IsAny<IEnumerable<EducatorsStaff>>())).
                Returns(GetTestEducatorsStaffDTO());

            //Act
            var result = await _educatorsStaffService.GetAllKVsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffDTO>>(result);

        }

        [Test]
        public async Task GetKadraById_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>()))
                .ReturnsAsync(new EducatorsStaff());
            _mapper.
                Setup(m => m.Map<EducatorsStaff, EducatorsStaffDTO>(It.IsAny<EducatorsStaff>())).
                Returns(new EducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffService.GetKadraById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffDTO>(result);
        }

        [Test]
        public async Task GetKadraByRegisterNumber_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>()))
                .ReturnsAsync(new EducatorsStaff());
            _mapper.
                Setup(m => m.Map<EducatorsStaff, EducatorsStaffDTO>(It.IsAny<EducatorsStaff>())).
                Returns(new EducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffService.GetKadraByRegisterNumber(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffDTO>(result);
        }

        [Test]
        public async Task GetKVsOfGivenUser_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new List<EducatorsStaff>().AsQueryable());
            _mapper.
                Setup(m => m.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(
                It.IsAny<IEnumerable<EducatorsStaff>>())).
                Returns(GetTestEducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffService.GetKVsOfGivenUser(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EducatorsStaffDTO>>(result);
        }

        [Test]
        public async Task DoesUserHaveSuchStaff_ReturnsFalse()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(() => null);

            // Act
            var result = await _educatorsStaffService.DoesUserHaveSuchStaff(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }
        [Test]
        public async Task DoesUserHaveSuchStaff_ReturnsTrue()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());

            // Act
            var result = await _educatorsStaffService.DoesUserHaveSuchStaff(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task StaffWithRegisternumberExists_ReturnsTrue()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());

            // Act
            var result = await _educatorsStaffService.StaffWithRegisternumberExists(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task StaffWithRegisternumberExists_ReturnsFalse()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(() => null);

            // Act
            var result = await _educatorsStaffService.StaffWithRegisternumberExists(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task StaffWithRegisternumberExistsEdit_ReturnsTrue()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());

            // Act
            var result = await _educatorsStaffService.StaffWithRegisternumberExistsEdit(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task StaffWithRegisternumberExistsEdit_ReturnsFalse()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(() => null);

            // Act
            var result = await _educatorsStaffService.StaffWithRegisternumberExistsEdit(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task SUserHasSuchStaffEdit_ReturnsTrue()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(GetTestEducatorsStaff());

            // Act
            var result = await _educatorsStaffService.UserHasSuchStaffEdit(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task UserHasSuchStaffEdit_ReturnsFalse()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(() => null);

            // Act
            var result = await _educatorsStaffService.UserHasSuchStaffEdit(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task GetUserByEduStaff_Returns1()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(eduStaff);

            // Act
            var result = await _educatorsStaffService.GetUserByEduStaff(eduStaff.ID);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(eduStaff.UserId, result);
        }

        [Test]
        public async Task GetKVsWithKVType_ReturnsEducatorsStaffDTO()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(GetTestEducatorsStaff());
            _mapper.
                Setup(m => m.Map<IEnumerable<EducatorsStaffDTO>>(It.IsAny<EducatorsStaff>())).
                Returns(GetTestEducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffService.GetKVsWithKVType(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffDTO[]>(result);
        }

        [Test]
        public async Task UpdateKadra_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());
            _repositoryWrapper.
                Setup(r => r.KVs.Update(It.IsAny<EducatorsStaff>()));
            _repositoryWrapper.
                Setup(r => r.SaveAsync());

            // Act
            await _educatorsStaffService.UpdateKadra(staffDTO);

            // Assert
            _repositoryWrapper.VerifyAll();
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
        private IEnumerable<EducatorsStaff> GetTestEducatorsStaff()
        {
            return new List<EducatorsStaff>
            {
                new EducatorsStaff{ID = 1},
                new EducatorsStaff{ID = 2},
                new EducatorsStaff{ID = 3}
            }.AsEnumerable();
        }

        private EducatorsStaff eduStaff => new EducatorsStaff
        {
            ID = 1,
            UserId = "2"
        };

        private EducatorsStaffDTO staffDTO => new EducatorsStaffDTO
        {
            ID = 1,
            NumberInRegister = 2,
        };
    }
}
