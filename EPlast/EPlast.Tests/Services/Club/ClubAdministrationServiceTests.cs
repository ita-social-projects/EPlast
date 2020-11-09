using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Services.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.Club
{
    [TestFixture]
    public class ClubAdministrationServiceTests
    {
        private ClubAdministrationService _clubAdministrationService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IAdminTypeService> _adminTypeService;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _clubAdministrationService = new ClubAdministrationService(_repoWrapper.Object, _mapper.Object, _adminTypeService.Object, _userManager.Object);
        }

        [Test]
        public async Task GetAdministrationByIdAsync_ReturnsAdministrations()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.ClubAdministration.GetAllAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                 It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                     .ReturnsAsync(new List<ClubAdministration> { new ClubAdministration() { ID = fakeId } });
            _mapper
                .Setup(m => m.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(It.IsAny<IEnumerable<ClubAdministration>>()))
                .Returns(new List<ClubAdministrationDTO> { new ClubAdministrationDTO { ID = fakeId } });

            // Act
            var result = await _clubAdministrationService.GetAdministrationByIdAsync(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.FirstOrDefault().ID, fakeId);
        }

        [Test]
        public async Task AddAdministratorAsync_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.ClubAdministration.CreateAsync(clubAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _clubAdministrationService.AddAdministratorAsync(clubAdmDTO);

            //Assert
            Assert.IsInstanceOf<ClubAdministrationDTO>(result);
        }

        [Test]
        public async Task EditAdministratorAsync_ReturnsEditedAdministratorWithSameId()
        {
            //Arrange
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
            _repoWrapper
                .Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAdministration>,
                IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            _repoWrapper
                .Setup(r => r.ClubAdministration.Update(It.IsAny<ClubAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _clubAdministrationService.EditAdministratorAsync(clubAdmDTO);

            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<ClubAdministrationDTO>(result);
        }

        [Test]
        public async Task EditAdministratorAsync_ReturnsEditedAdministratorWithDifferentId()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAdministration>,
                IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(new ClubAdministration());
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = 3
               });
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = 3
               });

            //Act
            var result = await _clubAdministrationService.EditAdministratorAsync(clubFakeAdmDTO);

            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<ClubAdministrationDTO>(result);
        }

        [Test]
        public void RemoveAdministratorAsync_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.ClubAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<ClubAdministration>,
                IIncludableQueryable<ClubAdministration, object>>>()))
                .ReturnsAsync(clubAdm);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminType));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.ClubAdministration.Update(It.IsAny<ClubAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _clubAdministrationService.RemoveAdministratorAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void CheckPreviousAdministratorsToDelete_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper.Setup(r => r.ClubAdministration.GetAllAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                 It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                     .ReturnsAsync(new List<ClubAdministration> { new ClubAdministration() { ID = fakeId } });
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = 3
               });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            //Act
            var result = _clubAdministrationService.CheckPreviousAdministratorsToDelete();

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }


        [Test]
        public async Task GetAdministrationsOfUserAsync_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper.Setup(r => r.ClubAdministration.GetAllAsync(It.IsAny<Expression<Func<ClubAdministration, bool>>>(),
                 It.IsAny<Func<IQueryable<ClubAdministration>, IIncludableQueryable<ClubAdministration, object>>>()))
                     .ReturnsAsync(new List<ClubAdministration> { new ClubAdministration() { ID = 3 } });
            _mapper
                .Setup(m => m.Map<IEnumerable<ClubAdministration>, IEnumerable<ClubAdministrationDTO>>(It.IsAny<IEnumerable<ClubAdministration>>()))
                .Returns(GetTestClubAdministration());

            //Act
            var result = await _clubAdministrationService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ClubAdministrationDTO>>(result);
        }

        private IEnumerable<ClubAdministrationDTO> GetTestClubAdministration()
        {
            return new List<ClubAdministrationDTO>
            {
                new ClubAdministrationDTO{UserId = "Голова Станиці"},
                new ClubAdministrationDTO{UserId = "Голова Станиці"}
            }.AsEnumerable();
        }

        private static AdminTypeDTO AdminType = new AdminTypeDTO
        {
            AdminTypeName = "Голова Станиці",
            ID = 1
        };

        private ClubAdministrationDTO clubAdmDTO = new ClubAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            ClubId = 1,
            AdminTypeId = 1,
            EndDate = DateTime.Today,
            StartDate = DateTime.Now,
            User = new ClubUserDTO(),
            UserId = "Голова Станиці"
        };

        private ClubAdministration clubAdm = new ClubAdministration
        {
            ID = 1,
            AdminType = new AdminType()
            {
                AdminTypeName = "Голова Станиці",
                ID = 1
            },
            AdminTypeId = AdminType.ID,
            UserId = "Голова Станиці"
        };

        private ClubAdministrationDTO clubFakeAdmDTO = new ClubAdministrationDTO
        {
            ID = 2,
            AdminType = AdminType,
            ClubId = 2,
            AdminTypeId = 2,
            EndDate = DateTime.Today,
            StartDate = DateTime.Now,
            User = new ClubUserDTO(),
            UserId = "Голова Станиці"
        };

        private int fakeId = 3;
    }
}
