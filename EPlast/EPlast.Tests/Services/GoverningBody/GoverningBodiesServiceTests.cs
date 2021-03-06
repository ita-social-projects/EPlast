﻿using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities;
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
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.GoverningBody;

namespace EPlast.Tests.Services.GoverningBody
{
    internal class GoverningBodiesServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GoverningBodiesService _governingBodiesService;
        private Mock<IUniqueIdService> _uniqueIdService;
        private Mock<IGoverningBodyBlobStorageRepository> _blobStorage;
        private Mock<ISecurityModel> _securityModel;
        private protected Mock<UserManager<User>> _userManager;
        private readonly int testId = 1;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobStorage = new Mock<IGoverningBodyBlobStorageRepository>();
            _uniqueIdService = new Mock<IUniqueIdService>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _securityModel = new Mock<ISecurityModel>();
            _governingBodiesService = new GoverningBodiesService(
                _repoWrapper.Object,
                _mapper.Object,
                _uniqueIdService.Object,
                _blobStorage.Object,
                _securityModel.Object);
        }

        [Test]
        public async Task GetOrganizationListAsync_ReturnsOrganizationsList()
        {
            //Arrange
            _repoWrapper
                .Setup(x => x.GoverningBody.GetAllAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(new List<Organization>());
            _mapper
                .Setup(x => x.Map<IEnumerable<GoverningBodyDTO>>(new List<Organization>())).Returns(new List<GoverningBodyDTO>());

            //Act
            var result = await _governingBodiesService.GetGoverningBodiesListAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDTO>>(result);
        }

        [Test]
        public async Task CreateAsync_Test()
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDTO>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDTO, Organization>(It.IsAny<GoverningBodyDTO>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));

            //Act
            var result = await _governingBodiesService.CreateAsync(testDTO);

            //Assert
            Assert.AreEqual(testDTO.Id, result);
            _repoWrapper.Verify(x => x.GoverningBody.CreateAsync(_mapper.Object.Map<Organization>(testDTO)), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_WithModel_ReturnsCityEdited()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDTO>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDTO, Organization>(It.IsAny<GoverningBodyDTO>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper.Setup(r => r.GoverningBody.Attach(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.GoverningBody.Update(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await _governingBodiesService.EditAsync(testDTO);

            // Assert
            _repoWrapper.Verify(r => r.GoverningBody.Attach(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [TestCase("logopath", "logo64path")]
        public async Task GetPhotoBase64_Valid_Test(string logoPath, string logo64Path)
        {
            //Arrange
            _blobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(logo64Path);

            //Act
            var result = await _governingBodiesService.GetLogoBase64Async(logoPath);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, logo64Path);
        }

        [TestCase(1)]
        public async Task GetProfileById(int id)
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization, GoverningBodyDTO>(It.IsAny<Organization>())).Returns(testDTO);
            _mapper
                .Setup(x => x.Map<GoverningBodyDTO, Organization>(It.IsAny<GoverningBodyDTO>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));

            //Act
            var result = await _governingBodiesService.GetGoverningBodyProfileAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(CreateGoverningBodyDTO.Id, result.GoverningBody.Id);
        }

        [TestCase(1)]
        public async Task GetProfileById_GoverningBodyNotFound(int id)
        {
            //Arrange
            Organization organization = null;
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Organization, bool>>>(), It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(organization);
            GoverningBodyDTO governingBodyDto = null;
            _mapper
                .Setup(x => x.Map<Organization, GoverningBodyDTO>(It.IsAny<Organization>()))
                .Returns(governingBodyDto);

            //Act
            var result = await _governingBodiesService.GetGoverningBodyProfileAsync(id);

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task RemoveAsync()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDTO>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Delete(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _blobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
            _repoWrapper.Verify(r => r.GoverningBody.Delete(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_WithoutLogo()
        {
            // Arrange
            _repoWrapper.Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(), null))
                .ReturnsAsync(CreateOrganizationWithoutLogo);
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Delete(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());

            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _blobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
            _repoWrapper.Verify(r => r.GoverningBody.Delete(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [TestCase("userId")]
        public async Task GetUserAccess_Test(string userId)
        {
            //Arrange
            var dict = new Dictionary<string, bool>()
            {
                {"action", true}
            };
            _securityModel.Setup(x => x.GetUserAccessAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(dict);

            //Act
            var result = await _governingBodiesService.GetUserAccessAsync(userId);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(dict.Count, result.Count);
        }

        [TestCase(1)]
        public async Task GetGoverningBodyDocumentsAsync_ReturnsDocuments(int governingBodyId)
        {
            //Arrange
            _mapper
                .Setup(x => x.Map<Organization, GoverningBodyDTO>(It.IsAny<Organization>())).Returns(CreateGoverningBodyDTO);
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(CreateGoverningBodyDTO));

            // Act
            var result = await _governingBodiesService.GetGoverningBodyDocumentsAsync(governingBodyId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<GoverningBodyProfileDTO>(result);
        }

        [TestCase(1)]
        public async Task GetGoverningBodyDocumentsAsync_WithGoverningBodyIsNull_ReturnNull(int governingBodyId)
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(CreateGoverningBodyDTO));
            _mapper.Setup(m => m.Map<Organization, GoverningBodyDTO>(It.IsAny<Organization>()))
                .Returns((GoverningBodyDTO)null);

            // Act
            var result = await _governingBodiesService.GetGoverningBodyDocumentsAsync(governingBodyId);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetAdministrationsOfUserAsync_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration> { new GoverningBodyAdministration() { Id = testId } });
            _mapper
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDTO>>(result);
        }

        [Test]
        public async Task GetAdministrationsOfUserAsync_WithGoverningBody_ReturnsCorrectAdministrations()
        {
            //Arrange
            _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration> { new GoverningBodyAdministration()
                {
                    Id = testId,
                    GoverningBody = new DataAccess.Entities.GoverningBody.Organization()
                } });
            _mapper
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDTO>>(result);
        }

        [Test]
        public async Task GetPreviousAdministrationsOfUserAsync_ReturnsCorrectAdministrations()
        {
            //Arrange
            _ = _repoWrapper
                .Setup(r => r.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration> { new GoverningBodyAdministration()
                {
                    Id = testId,
                    GoverningBody = new DataAccess.Entities.GoverningBody.Organization ()
                } });
            _mapper
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDTO>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDTO>>(result);
        }

        private IEnumerable<GoverningBodyAdministrationDTO> GetTestGoverningBodyAdministration()
        {
            return new List<GoverningBodyAdministrationDTO>
            {
                new GoverningBodyAdministrationDTO{UserId = Roles.GoverningBodyHead},
                new GoverningBodyAdministrationDTO{UserId = Roles.GoverningBodyHead}
            }.AsEnumerable();
        }
        
        private GoverningBodyDTO CreateGoverningBodyDTO => new GoverningBodyDTO()
        {
            Id = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = "daa1-4d27-b94d-/9ab2d890d9d0.jpeg,63cc77aa,",
            PhoneNumber = "12345",
            GoverningBodyAdministration = new List<GoverningBodyAdministrationDTO>
            {
                new GoverningBodyAdministrationDTO
                {
                    AdminType = new AdminTypeDTO
                    {
                        AdminTypeName = Roles.GoverningBodyHead
                    }
                },
                new GoverningBodyAdministrationDTO
                {
                    AdminType = new AdminTypeDTO
                    {
                        AdminTypeName = Roles.GoverningBodySecretary
                    }
                }
            },
            GoverningBodyDocuments = new List<GoverningBodyDocumentsDTO>
            {
                new GoverningBodyDocumentsDTO()
            }
        };

        private Organization CreateOrganizationWithoutLogo => new Organization()
        {
            ID = 1,
            OrganizationName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = null,
            PhoneNumber = "12345"
        };
    }
}
