﻿using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.Tests.Services.City
{
    [TestFixture]
    public class CityParticipantsServiceTests
    {
        private static readonly AdminTypeDTO AdminType = new AdminTypeDTO
        {
            AdminTypeName = "Голова Станиці",
            ID = 1
        };
        private readonly int anotherFakeId = 2;
        private readonly CityAdministration cityAdm = new CityAdministration
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
        private readonly CityAdministrationDTO cityAdmDTO = new CityAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            CityId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Today,
            User = new CityUserDTO(),
            UserId = "Голова Станиці"
        };
        private readonly int fakeId = 3;
        private Mock<IAdminTypeService> _adminTypeService;
        private ICityParticipantsService _cityParticipantsService;
        private Mock<IEmailSendingService> _emailSendingService;
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;

        [Test]
        public async Task AddAdministratorAsync_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTO);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task AddAdministratorAsync_WhereStartDateIsNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
            cityAdmDTO.StartDate = null;

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTO);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
            Assert.Null(result.StartDate);
        }

        [Test]
        public async Task AddFollowerAsync_ReturnsCityMembersDTO()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>, IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repoWrapper
                .Setup(x => x.CityMembers.Delete(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.SaveAsync());
            _repoWrapper
                .Setup(x => x.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(GetCityAdministration());
            _repoWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration() { AdminTypeId = 2 });
            _adminTypeService
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>())).ReturnsAsync(new AdminTypeDTO() { AdminTypeName = "Голова Станиці" });
            _repoWrapper
                .Setup(x => x.CityAdministration.Update(new CityAdministration()));
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _repoWrapper
                .Setup(x => x.CityMembers.CreateAsync(It.IsAny<CityMembers>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _mapper
                .Setup(x => x.Map<CityMembers, CityMembersDTO>(It.IsAny<CityMembers>())).Returns(new CityMembersDTO());
            // Act
            var result = await _cityParticipantsService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>());
            // Assert
            Assert.IsInstanceOf<CityMembersDTO>(result);
        }

        [Test]
        public async Task AddFollowerAsyncWithUser_Valit_Test()
        {
            //Arrange
            _userManager
                .Setup(u => u.GetUserIdAsync(It.IsAny<User>()))
                .ReturnsAsync("1");
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>, IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repoWrapper
                .Setup(x => x.CityMembers.Delete(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.SaveAsync());
            _repoWrapper
                .Setup(x => x.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(GetCityAdministration());
            _repoWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration() { AdminTypeId = 2 });
            _adminTypeService
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>())).ReturnsAsync(new AdminTypeDTO() { AdminTypeName = "Голова Станиці" });
            _repoWrapper
                .Setup(x => x.CityAdministration.Update(new CityAdministration()));
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _repoWrapper
                .Setup(x => x.CityMembers.CreateAsync(It.IsAny<CityMembers>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _mapper
                .Setup(x => x.Map<CityMembers, CityMembersDTO>(It.IsAny<CityMembers>())).Returns(new CityMembersDTO());

            // Act
            var result = await _cityParticipantsService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<User>());

            // Assert
            Assert.IsInstanceOf<CityMembersDTO>(result);
        }

        [Test]
        public void CheckPreviousAdministratorsToDelete_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId } });
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = fakeId
               });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            //Act
            var result = _cityParticipantsService.CheckPreviousAdministratorsToDelete();

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void CheckPreviousAdministratorsToDelete_WithDifferrentAdminTypeId_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId, AdminTypeId = fakeId } })
                .Callback(() => _repoWrapper
                    .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                        It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                    .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId, AdminTypeId = anotherFakeId } }));
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = fakeId
               });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            //Act
            var result = _cityParticipantsService.CheckPreviousAdministratorsToDelete();

            //Assert
            _repoWrapper.Verify();
            _userManager.Verify(u => u.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public void CheckPreviousAdministratorsToDelete_WithDifferrentIDAndAdminTypeId_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId, AdminTypeId = fakeId } })
                .Callback(() => _repoWrapper
                    .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                        It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                    .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId, AdminTypeId = anotherFakeId } }));
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = anotherFakeId
               });
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));

            //Act
            var result = _cityParticipantsService.CheckPreviousAdministratorsToDelete();

            //Assert
            _repoWrapper.Verify();
            _userManager.Verify(u => u.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManager.Verify(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
        }

        [Test]
        public async Task EditAdministratorAsync_ReturnsEditedAdministratorWithSameId()
        {
            //Arrange
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _repoWrapper
                .Setup(r => r.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTO);

            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task EditAdministratorAsync_WhereStartTimeIsNull_ReturnsEditedAdministratorWithSameId()
        {
            //Arrange
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new AdminTypeDTO());
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _repoWrapper
                .Setup(r => r.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());
            cityAdmDTO.StartDate = null;

            //Act
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTO);

            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
            Assert.Null(result.StartDate);
        }

        [Test]
        public async Task EditAdministratorAsync_WithDifferentAdminTypeId_ReturnsEditedAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration());
            _adminTypeService
               .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = fakeId
               });
            _adminTypeService
               .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = "Голова Станиці",
                   ID = fakeId
               });

            //Act
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTO);

            //Assert
            _repoWrapper.Verify();
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task GetAdministrationByIdAsync_ReturnsAdministrations()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId } });
            _mapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(new List<CityAdministrationDTO> { new CityAdministrationDTO { ID = fakeId, AdminType = new AdminTypeDTO(), User = new CityUserDTO() } });

            // Act
            var result = await _cityParticipantsService.GetAdministrationByIdAsync(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Select(admin => admin.User));
            Assert.NotNull(result.Select(admin => admin.AdminType));
            Assert.AreEqual(result.FirstOrDefault().ID, fakeId);
        }

        [Test]
        public async Task GetAdministrationsOfUserAsync_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId } });
            _mapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(GetTestCityAdministration());

            //Act
            var result = await _cityParticipantsService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationDTO>>(result);
        }

        [Test]
        public async Task GetAdministrationsOfUserAsync_WithCity_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration()
                {
                    ID = fakeId,
                    City = new DataAccess.Entities.City ()
                } });
            _mapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(GetTestCityAdministration());

            //Act
            var result = await _cityParticipantsService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationDTO>>(result);
        }

        [Test]
        public async Task GetAdministrationStatuses_ReturnsCorrectAdministrationStatuses()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration()
                {
                    ID = fakeId
                } });
            _mapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationStatusDTO>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(GetTestCityAdministrationStatuses());

            //Act
            var result = await _cityParticipantsService.GetAdministrationStatuses(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationStatusDTO>>(result);
        }

        [Test]
        public async Task GetByCityIdAsyncCorrect()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityAdministration>, IIncludableQueryable<DataAccess.Entities.CityAdministration, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.CityAdministration> { new DataAccess.Entities.CityAdministration() });

            // Act
            await _cityParticipantsService.GetAdministrationByIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DataAccess.Entities.CityAdministration>, IEnumerable<CityAdministrationDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.CityAdministration>>()));
        }

        [Test]
        public async Task GetCurrentByCityIdAsyncCorrect()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>, IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.CityMembers> { new DataAccess.Entities.CityMembers() });

            // Act
            await _cityParticipantsService.GetMembersByCityIdAsync(It.IsAny<int>());

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<DataAccess.Entities.CityMembers>, IEnumerable<CityMembersDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.CityMembers>>()));
        }

        [Test]
        public async Task GetMembersByCityIdAsync_ReturnsMembers()
        {
            // Arrange
            _repoWrapper.Setup(r => r.CityMembers.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>, IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.CityMembers> { new DataAccess.Entities.CityMembers() });

            // Act
            var result = await _cityParticipantsService.GetMembersByCityIdAsync(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            _mapper.Verify(m => m.Map<IEnumerable<DataAccess.Entities.CityMembers>, IEnumerable<CityMembersDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.CityMembers>>()));
        }

        [Test]
        public async Task GetPreviousAdministrationsOfUserAsync_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new List<CityAdministration> { new CityAdministration()
                {
                    ID = fakeId,
                    City = new DataAccess.Entities.City ()
                } });
            _mapper
                .Setup(m => m.Map<IEnumerable<CityAdministration>, IEnumerable<CityAdministrationDTO>>(It.IsAny<IEnumerable<CityAdministration>>()))
                .Returns(GetTestCityAdministration());

            //Act
            var result = await _cityParticipantsService.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<CityAdministrationDTO>>(result);
        }

        [Test]
        public void RemoveAdministratorAsync_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(cityAdm);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminType));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _cityParticipantsService.RemoveAdministratorAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void RemoveAdministratorAsync_WithAnotherRole_ReturnsCorrect()
        {
            //Arrange
            AdminType.AdminTypeName = "Another";
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(cityAdm);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminType));
            _userManager
                .Setup(u => u.FindByIdAsync(It.IsAny<string>()));
            _userManager
                .Setup(u => u.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _repoWrapper
                .Setup(r => r.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = _cityParticipantsService.RemoveAdministratorAsync(It.IsAny<int>());

            //Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task RemoveFollowerAsync_Valid_Test()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<DataAccess.Entities.CityMembers>, IIncludableQueryable<DataAccess.Entities.CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repoWrapper
                .Setup(x => x.CityMembers.Delete(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.SaveAsync());

            // Act
            await _cityParticipantsService.RemoveFollowerAsync(fakeId);

            // Assert
            _repoWrapper.Verify();
        }

        [Test]
        public async Task RemoveMemberAsync_Valid_Test()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.CityMembers.Delete(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.SaveAsync());

            // Act
            await _cityParticipantsService.RemoveMemberAsync(new CityMembers());

            // Assert
            _repoWrapper.Verify();
        }

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _adminTypeService = new Mock<IAdminTypeService>();
            _user = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_user.Object, null, null, null, null, null, null, null, null);
            _emailSendingService = new Mock<IEmailSendingService>();
            _cityParticipantsService = new CityParticipantsService(_repoWrapper.Object, _mapper.Object, _userManager.Object, _adminTypeService.Object, _emailSendingService.Object);
        }

        [Test]
        public async Task ToggleApproveStatusAsync_Valid_Test()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.CityMembers
                             .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                                                     It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers()
                {
                    UserId = fakeId.ToString(),
                    User = new User() { Email = "email" },
                    City = new DataAccess.Entities.City() { Name = "CityName" },
                    IsApproved = false
                });
            _repoWrapper
                .Setup(x => x.CityMembers.Update(It.IsAny<CityMembers>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());
            _userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            _repoWrapper
                .Setup(m => m.UserMembershipDates
                             .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserMembershipDates, bool>>>(),
                                                     It.IsAny<Func<IQueryable<UserMembershipDates>, IIncludableQueryable<UserMembershipDates, object>>>()))
                .ReturnsAsync(new UserMembershipDates());
            _repoWrapper
                .Setup(x => x.UserMembershipDates.Update(It.IsAny<UserMembershipDates>()));
            _mapper
                .Setup(x => x.Map<CityMembers, CityMembersDTO>(It.IsAny<CityMembers>()))
                .Returns(new CityMembersDTO());
            _repoWrapper
                .Setup(x => x.GetCitiesUrl)
                .Returns("citiesUrl");
            _emailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>(),
                                             It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _cityParticipantsService.ToggleApproveStatusAsync(fakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityMembersDTO>(result);
        }

        private IEnumerable<CityAdministration> GetCityAdministration()
        {
            return new List<CityAdministration>
            {
                new CityAdministration{UserId = "Голова Станиці", ID=2},
                new CityAdministration{UserId = "Голова Станиці", ID=3}
            }.AsEnumerable();
        }

        private IEnumerable<CityAdministrationDTO> GetTestCityAdministration()
        {
            return new List<CityAdministrationDTO>
            {
                new CityAdministrationDTO{UserId = "Голова Станиці"},
                new CityAdministrationDTO{UserId = "Голова Станиці"}
            }.AsEnumerable();
        }

        private IEnumerable<CityAdministrationStatusDTO> GetTestCityAdministrationStatuses()
        {
            return new List<CityAdministrationStatusDTO>
            {
                new CityAdministrationStatusDTO{UserId = "Голова Станиці"},
                new CityAdministrationStatusDTO{UserId = "Голова Станиці"}
            }.AsEnumerable();
        }
    }
}
