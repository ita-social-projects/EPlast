using AutoMapper;
using EPlast.BLL.Services.EducatorsStaff;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
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
        private Mock<UserManager<User>> _userManager;

        private EducatorsStaffService _educatorsStaffService;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();

            _educatorsStaffService = new EducatorsStaffService(
                _repositoryWrapper.Object,
                _userManager.Object,
                _mapper.Object);
        }

        [Test]
        public async Task CreateKadra_Test()
        {
            // Arange
            _userManager
              .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
              .ReturnsAsync(new User());

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
            _userManager
             .Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.Admin });
            var kadrasDTO = new EducatorsStaffDTO()
            {
                UserId = "test"
            };
            // Act
            var result = await _educatorsStaffService.
                CreateKadra(kadrasDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<EducatorsStaffDTO>(result);
        }

        [Test]
        public void Addkadra_UserHasRestrictedRoles_ThrowsArgumentException_Test()
        {
            //Arrange

            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { Roles.RegisteredUser, Roles.Supporter });
            _mapper.Setup(x => x.Map<EducatorsStaffDTO, EducatorsStaff>
                         (It.IsAny<EducatorsStaffDTO>())).
                          Returns(new EducatorsStaff());

            //Act  //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _educatorsStaffService.CreateKadra(new EducatorsStaffDTO() { UserId = "test" }));
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
        public void UpdateKadra_RegisternumberExists_ThrowsArgumentException_Test()
        {
            // Arrange
            _repositoryWrapper.
                Setup(r => r.KVs.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(new EducatorsStaff());

            //Act  //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _educatorsStaffService.UpdateKadra(new EducatorsStaffDTO() { KadraVykhovnykivTypeId = It.IsAny<int>(), NumberInRegister = It.IsAny<int>() }));
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
        public void CreateKadra_UserHasSuchStaff_ThrowsArgumentException_Test()
        {
            // Arrange
            _userManager
               .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
               .ReturnsAsync(new List<string> { Roles.Admin });
            _repositoryWrapper.
                Setup(r => r.KVs.GetAllAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
                It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>())).
                ReturnsAsync(GetTestEducatorsStaff());

            //Act  //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _educatorsStaffService.CreateKadra(new EducatorsStaffDTO() { UserId = It.IsAny<string>(), KadraVykhovnykivTypeId = It.IsAny<int>() }));
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

        [Test]
        public void GetEducatorsStaffTableObject_ReturnsEducatorsStaffTableObject()
        {
            //Arrange
            _repositoryWrapper
                .Setup(r => r.KVs.GetEducatorsStaff(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(new List<EducatorsStaffTableObject>());

            //Act
            var result = _educatorsStaffService.GetEducatorsStaffTableObject(It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<EducatorsStaffTableObject>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByDateAscend()
        {
            //Arrange
            string[] SortByOrder = new [] { "endDate", "ascend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByDateDescend()
        {
            //Arrange
            string[] SortByOrder = new[] { "endDate", "descend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByUserNameDescend()
        {
            //Arrange
            string[] SortByOrder = new[] { "userName", "descend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByUserNameAscend()
        {
            //Arrange
            string[] SortByOrder = new[] { "userName", "ascend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByNumberInRegisterDescend()
        {
            //Arrange
            string[] SortByOrder = new[] { "numberInRegister", "descend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByNumberInRegisterAscend()
        {
            //Arrange
            string[] SortByOrder = new[] { "numberInRegister", "ascend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByIdAscend()
        {
            //Arrange
            string[] SortByOrder = new[] { "id", "ascend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllUsersEducatorsStaffByPageAsync_ReturnsTupleWithUserEducatorsStaffTableObjectAndIntRowsSortedByIdDescend()
        {
            //Arrange
            string[] SortByOrder = new[] { "id", "descend" };

            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(It.IsAny<int>(), SortByOrder, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllEducatorsStaffByPageAsync_ReturnsTupleWithEducatorsStaffTableObjectAndIntRowsWithFilters()
        {
            //Arrange
            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(5, new List<string> { "id", "ascend" }, "", 1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public async Task GetAllEducatorsStaffByPageAsync_ReturnsTupleWithEducatorsStaffTableObjectAndRowsWithFilters()
        {
            //Arrange
            _repositoryWrapper
              .Setup(x => x.KVs.GetRangeAsync(It.IsAny<Expression<Func<EducatorsStaff, bool>>>(),
              It.IsAny<Expression<Func<EducatorsStaff, EducatorsStaff>>>(), It.IsAny<Func<IQueryable<EducatorsStaff>, IQueryable<EducatorsStaff>>>(),
              It.IsAny<Func<IQueryable<EducatorsStaff>, IIncludableQueryable<EducatorsStaff, object>>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(CreateTuple);

            //Act
            var result = await _educatorsStaffService.GetEducatorsStaffTableAsync(5, new List<string> { "id", "ascend" }, It.IsAny<string>(), 1, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<EducatorsStaffTableObject>, int>>(result);
        }
        [Test]
        public void GetOrderByUserName()
        {
            //Arrange
            //Act
            var result = _educatorsStaffService.GetOrderByUserName();

            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetOrderByID()
        {
            //Arrange
            //Act
            var result = _educatorsStaffService.GetOrderByID();

            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetOrderByNumberInRegister()
        {
            //Arrange
            //Act
            var result = _educatorsStaffService.GetOrderByNumberInRegister();

            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetOrderByDateOfGranting()
        {
            //Arrange
            //Act
            var result = _educatorsStaffService.GetOrderByDateOfGranting();

            //Assert
            Assert.IsNotNull(result);
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
        private List<EducatorsStaff> GetUsersByPage()
        {
            return new List<EducatorsStaff>()
            {
                new EducatorsStaff()
                {
                    NumberInRegister = 34,
                }
            };
        }

        private int GetFakeUserNumber()
        {
            return 100;
        }
        private Tuple<IEnumerable<EducatorsStaff>, int> CreateTuple => new Tuple<IEnumerable<EducatorsStaff>, int>(GetUsersByPage(), GetFakeUserNumber());
    }
}
