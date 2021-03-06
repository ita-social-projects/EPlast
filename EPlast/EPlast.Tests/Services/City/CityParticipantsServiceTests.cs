﻿using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Models;
using EPlast.BLL.Services.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
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
            AdminTypeName = Roles.CityHead,
            ID = 1
        };

        private static readonly AdminTypeDTO AdminDeputyType = new AdminTypeDTO
        {
            AdminTypeName = Roles.CityHeadDeputy,
            ID = 1
        };

        private static readonly AdminTypeDTO AdminSecretaryType = new AdminTypeDTO
        {
            AdminTypeName = Roles.CitySecretary,
            ID = 1
        };

        private readonly CityAdministration cityAdm = new CityAdministration
        {
            ID = 1,
            AdminType = new AdminType()
            {
                AdminTypeName = Roles.CityHead,
                ID = 1
            },
            AdminTypeId = AdminType.ID,
            UserId = Roles.CityHead
        };

        private readonly CityAdministrationDTO cityAdmDTOEndDateToday = new CityAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            CityId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Today,
            User = new CityUserDTO(),
            UserId = Roles.CityHead
        };

        private readonly CityAdministrationDTO cityAdmDTOEndDateNull = new CityAdministrationDTO
        {
            ID = 1,
            AdminType = AdminType,
            CityId = 1,
            AdminTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = null,
            User = new CityUserDTO(),
            UserId = Roles.CityHead
        };

        private readonly int fakeId = 3;
        private Mock<IAdminTypeService> _adminTypeService;
        private ICityParticipantsService _cityParticipantsService;
        private Mock<IEmailSendingService> _emailSendingService;
        private Mock<IEmailContentService> _emailContentService;
        private Mock<IMapper> _mapper;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IUserStore<User>> _user;
        private Mock<UserManager<User>> _userManager;

        [Test]
        public async Task AddAdministratorAsync_EndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminType);

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateToday);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task AddAdministratorAsync_EndDateToday_AdminDeputyType_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminDeputyType);

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateToday);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task AddAdministratorAsync_EndDateToday_AdminSecretaryType_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminSecretaryType);

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateToday);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task AddAdministratorAsync_EndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminType);

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateNull);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
        }

        [Test]
        public async Task AddAdministratorAsync_WhereStartDateIsNullWithEndDateToday_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminType);
            cityAdmDTOEndDateToday.StartDate = null;

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateToday);

            //Assert
            Assert.IsInstanceOf<CityAdministrationDTO>(result);
            Assert.Null(result.StartDate);
        }

        [Test]
        public async Task AddAdministratorAsync_WhereStartDateIsNullWithEndDateNull_ReturnsAdministrator()
        {
            //Arrange
            _repoWrapper
                .Setup(s => s.CityAdministration.CreateAsync(cityAdm));
            _adminTypeService
                .Setup(a => a.GetAdminTypeByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminType);
            cityAdmDTOEndDateNull.StartDate = null;

            //Act
            var result = await _cityParticipantsService.AddAdministratorAsync(cityAdmDTOEndDateNull);

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
            _repoWrapper
                .Setup(x => x.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(new CityAdministration() { AdminTypeId = 76 });
            _adminTypeService
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>())).ReturnsAsync(new AdminTypeDTO() { AdminTypeName = Roles.CityHead });
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
            _userManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _emailContentService
                .Setup(x => x.GetCityAdminAboutNewFollowerEmailAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new EmailModel());

            // Act
            var result = await _cityParticipantsService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<string>());

            // Assert
            Assert.IsInstanceOf<CityMembersDTO>(result);
        }

        [Test]
        public async Task AddFollowerAsyncWithUser_Valid_Test()
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
                .Setup(x => x.GetAdminTypeByIdAsync(It.IsAny<int>())).ReturnsAsync(new AdminTypeDTO() { AdminTypeName = Roles.CityHead });
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
        public void ContinueAdminsDueToDate_EndDateEarlier_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
               .Setup(x => x.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                   It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
               .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId, EndDate = new DateTime(2001, 7, 20) } });
            _repoWrapper
                .Setup(x => x.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());

            //Act
            var result = _cityParticipantsService.ContinueAdminsDueToDate();

            //Assert
            _repoWrapper.Verify(x => x.SaveAsync());
            _repoWrapper.Verify(x => x.CityAdministration.Update(It.IsAny<CityAdministration>()));
            Assert.NotNull(result);
        }

        [Test]
        public void ContinueAdminsDueToDate_EndDateNull_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
               .Setup(x => x.CityAdministration.GetAllAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                   It.IsAny<Func<IQueryable<CityAdministration>, IIncludableQueryable<CityAdministration, object>>>()))
               .ReturnsAsync(new List<CityAdministration> { new CityAdministration() { ID = fakeId } });
            _repoWrapper
                .Setup(x => x.CityAdministration.Update(It.IsAny<CityAdministration>()));
            _repoWrapper
                .Setup(x => x.SaveAsync());

            //Act
            var result = _cityParticipantsService.ContinueAdminsDueToDate();

            //Assert
            _repoWrapper.Verify(x => x.SaveAsync());
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
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTOEndDateToday);

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
            cityAdmDTOEndDateToday.StartDate = null;

            //Act
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTOEndDateToday);

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
                   AdminTypeName = Roles.CityHead,
                   ID = fakeId
               });
            _adminTypeService
               .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new AdminTypeDTO
               {
                   AdminTypeName = Roles.CityHead,
                   ID = fakeId
               });

            //Act
            var result = await _cityParticipantsService.EditAdministratorAsync(cityAdmDTOEndDateToday);

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
        public void RemoveAdministratorAsync_AdminType_ReturnsCorrect()
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
        public void RemoveAdministratorAsync_AdminDeputyType_ReturnsCorrect()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.CityAdministration.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<CityAdministration>,
                    IIncludableQueryable<CityAdministration, object>>>()))
                .ReturnsAsync(cityAdm);
            _adminTypeService
                .Setup(a => a.GetAdminTypeByIdAsync(It.IsAny<int>()))
                .Returns(() => Task<AdminTypeDTO>.Factory.StartNew(() => AdminDeputyType));
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

        [TestCase("email", "CityName")]
        public async Task RemoveFollowerAsync_Valid_Test(string email, string cityName)
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.CityMembers
                    .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                        It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers
                {
                    UserId = fakeId.ToString(),
                    User = new User { Email = email },
                    City = new DataAccess.Entities.City { Name = cityName },
                    IsApproved = false
                });
            _repoWrapper
                .Setup(x => x.CityMembers.Delete(It.IsAny<CityMembers>()));
            _repoWrapper
               .Setup(x => x.SaveAsync());
            _emailSendingService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);
            _emailContentService
                .Setup(x => x.GetCityRemoveFollowerEmail(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new EmailModel());

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
            _emailContentService = new Mock<IEmailContentService>();
            _cityParticipantsService = new CityParticipantsService(_repoWrapper.Object, _mapper.Object, _userManager.Object, _adminTypeService.Object, _emailSendingService.Object, _emailContentService.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ToggleApproveStatusAsync_Valid_Test(bool isInRole)
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
                .ReturnsAsync(isInRole);
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
            _emailContentService
                .Setup(x => x.GetCityApproveEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new EmailModel());
            _emailContentService
                .Setup(x => x.GetCityToSupporterRoleOnApproveEmail()).Returns(new EmailModel());

            // Act
            var result = await _cityParticipantsService.ToggleApproveStatusAsync(fakeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<CityMembersDTO>(result);
        }
        [Test]
        public async Task CityOfApprovedMemberTest()
        {
            //Arrange 
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                             It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers() 
                {
                    ID = 1,
                    UserId = "123v",
                    IsApproved = true,
                    City = new DataAccess.Entities.City
                    {
                        ID = 1,
                        Name = "city name"
                    },
                    CityId = 1
                });
            //Act
            var result = await _cityParticipantsService.CityOfApprovedMember("123v");
            //Assert
            Assert.IsNotNull(result);
            _repoWrapper.Verify();
        }

        [Test]
        public async Task CityOfApprovedMemberTest_ReturnsNull()
        {
            //Arrange 
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync((null as CityMembers));
            //Act
            var result = await _cityParticipantsService.CityOfApprovedMember("123v");
            //Assert
            Assert.IsNull(result);
            _repoWrapper.Verify();
        }

        [Test]
        public async Task CityOfApprovedMemberTest_ReturnsNullName()
        {
            //Arrange 
            _repoWrapper
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers()
                {
                    ID = 1,
                    UserId = "123v",
                    IsApproved = false,
                    City = new DataAccess.Entities.City
                    {
                        ID = 1,
                        Name = "city name"
                    },
                    CityId = 1
                });
            //Act
            var result = await _cityParticipantsService.CityOfApprovedMember("123v");
            //Assert
            Assert.IsNull(result);
            _repoWrapper.Verify();
        }

        private IEnumerable<CityAdministration> GetCityAdministration()
        {
            return new List<CityAdministration>
            {
                new CityAdministration
                {
                    UserId = "userId",
                    ID = 2,
                    AdminType = new AdminType
                    {
                        AdminTypeName = Roles.CityHead
                    },
                    User = new User()
                },
                new CityAdministration
                {
                    UserId = "userId",
                    ID = 3,
                    AdminType = new AdminType
                    {
                        AdminTypeName = Roles.CityHead
                    },
                    User = new User()
                },
                new CityAdministration
                {
                    UserId = "userId",
                    ID = 4,
                    AdminType = new AdminType
                    {
                        AdminTypeName = Roles.CityHeadDeputy
                    },
                    User = new User()
                }
            }.AsEnumerable();
        }

        private IEnumerable<CityAdministrationDTO> GetTestCityAdministration()
        {
            return new List<CityAdministrationDTO>
            {
                new CityAdministrationDTO{UserId = Roles.CityHead},
                new CityAdministrationDTO{UserId = Roles.CityHead}
            }.AsEnumerable();
        }

        private IEnumerable<CityAdministrationStatusDTO> GetTestCityAdministrationStatuses()
        {
            return new List<CityAdministrationStatusDTO>
            {
                new CityAdministrationStatusDTO{UserId = Roles.CityHead},
                new CityAdministrationStatusDTO{UserId = Roles.CityHead}
            }.AsEnumerable();
        }
    }
}
