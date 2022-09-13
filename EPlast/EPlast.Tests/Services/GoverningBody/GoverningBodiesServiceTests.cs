using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.GoverningBody
{
    internal class GoverningBodiesServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GoverningBodiesService _governingBodiesService;
        private Mock<IGoverningBodyAdministrationService> _governingBodyAdministrationService;
        private Mock<ISectorService> _sectorService;
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
            _governingBodyAdministrationService = new Mock<IGoverningBodyAdministrationService>();
            _sectorService = new Mock<ISectorService>();
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _securityModel = new Mock<ISecurityModel>();
            _governingBodiesService = new GoverningBodiesService(
                _repoWrapper.Object,
                _mapper.Object,
                _blobStorage.Object,
                _securityModel.Object,
                _governingBodyAdministrationService.Object,
                _sectorService.Object
            );
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
                .Setup(x => x.Map<IEnumerable<GoverningBodyDto>>(new List<Organization>())).Returns(new List<GoverningBodyDto>());

            //Act
            var result = await _governingBodiesService.GetGoverningBodiesListAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDto>>(result);
        }


        [Test]
        public async Task GetSectorsListAsync_withValidId_ReturnsSectorsList()
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization, GoverningBodyDto>(It.IsAny<Organization>())).Returns(testDTO);
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            _mapper
                .Setup(x => x.Map<IEnumerable<GoverningBodyDto>>(new GoverningBodyDto())).Returns(new List<GoverningBodyDto>());

            //Act
            var result = await _governingBodiesService.GetSectorsListAsync(It.IsAny<int>());
            //Assert
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDto>>(result);
        }

        [Test]
        public void CreateAsync_GBWithSameNameExists_ThrowsArgumentException()
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>()))
                .Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            ;
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _governingBodiesService.CreateAsync(testDTO));
        }

        [Test]
        public async Task CreateAsync_Test()
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>()))
                .Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(null as Organization);

            //Act
            var result = await _governingBodiesService.CreateAsync(testDTO);

            //Assert
            Assert.AreEqual(testDTO.Id, result);
            _repoWrapper.Verify(x => x.GoverningBody.CreateAsync(_mapper.Object.Map<Organization>(testDTO)), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Once);
            _blobStorage.Verify(r => r.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task EditAsync_WithModel_ReturnsCityEdited()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>()))
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

        [Test]
        public async Task EditAsync_WithNotEmptyImageName_ShouldDeleteBlob()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper.Setup(r => r.GoverningBody.Attach(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.GoverningBody.Update(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());
            _repoWrapper.Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));

            // Act
            await _governingBodiesService.EditAsync(testDTO);

            // Assert
            _repoWrapper.Verify(r => r.GoverningBody.Attach(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _blobStorage.Verify(r => r.DeleteBlobAsync(It.IsAny<string>()), Times.Once);
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
                .Setup(x => x.Map<Organization, GoverningBodyDto>(It.IsAny<Organization>())).Returns(testDTO);
            _mapper
                .Setup(x => x.Map<GoverningBodyDto, Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
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
            GoverningBodyDto governingBodyDto = null;
            _mapper
                .Setup(x => x.Map<Organization, GoverningBodyDto>(It.IsAny<Organization>()))
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
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Delete(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(null as IEnumerable<GoverningBodyAdministration>);

            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_WithSectors()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Delete(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(null as IEnumerable<GoverningBodyAdministration>);
            _sectorService.Setup(r => r.GetSectorsByGoverningBodyAsync(It.IsAny<int>()))
                .ReturnsAsync(Sectors);
            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _sectorService.Verify(r => r.RemoveAsync(It.IsAny<int>()), Times.Exactly(Sectors.Count));
        }

        [Test]
        public async Task RemoveAsync_HasAdmins()
        {
            // Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDto>())).Returns(new Organization() { ID = testDTO.Id, Logo = testDTO.Logo });
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Update(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());
            _repoWrapper
                .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration>() { new GoverningBodyAdministration() { Id = 1 } });

            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
            _governingBodyAdministrationService.Verify(r => r.RemoveAdministratorAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_WithoutLogo()
        {
            // Arrange
            _repoWrapper.Setup(r => r.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(), null))
                .ReturnsAsync(CreateOrganizationWithoutLogo);
            _blobStorage.Setup(c => c.DeleteBlobAsync(It.IsAny<string>()));
            _repoWrapper.Setup(r => r.GoverningBody.Update(It.IsAny<Organization>()));
            _repoWrapper.Setup(r => r.SaveAsync());
            _repoWrapper
              .Setup(x => x.GoverningBodyAdministration.GetAllAsync(It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                  It.IsAny<Func<IQueryable<GoverningBodyAdministration>, IIncludableQueryable<GoverningBodyAdministration, object>>>()))
              .ReturnsAsync(null as IEnumerable<GoverningBodyAdministration>);

            // Act
            await _governingBodiesService.RemoveAsync(It.IsAny<int>());

            // Assert
            _blobStorage.Verify(c => c.DeleteBlobAsync(It.IsAny<string>()), Times.Never);
            _repoWrapper.Verify(r => r.GoverningBody.Update(It.IsAny<Organization>()), Times.Once);
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
                .Setup(x => x.Map<Organization, GoverningBodyDto>(It.IsAny<Organization>())).Returns(CreateGoverningBodyDTO);
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(CreateGoverningBodyDTO));

            // Act
            var result = await _governingBodiesService.GetGoverningBodyDocumentsAsync(governingBodyId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<GoverningBodyProfileDto>(result);
        }

        [TestCase(1)]
        public async Task GetGoverningBodyDocumentsAsync_WithGoverningBodyIsNull_ReturnNull(int governingBodyId)
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(CreateGoverningBodyDTO));
            _mapper.Setup(m => m.Map<Organization, GoverningBodyDto>(It.IsAny<Organization>()))
                .Returns((GoverningBodyDto)null);

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
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDto>>(result);
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
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDto>>(result);
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
                .Setup(m => m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(GetTestGoverningBodyAdministration());

            //Act
            var result = await _governingBodiesService.GetPreviousAdministrationsOfUserAsync(It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GoverningBodyAdministrationDto>>(result);
        }

        [Test]
        public async Task GetAdministrationForTableAsync_ReturnsDataForTable()
        {
            //Arrange
            _repoWrapper
                .Setup(g => g.GoverningBodyAdministration.GetAllAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(new List<GoverningBodyAdministration>());
            _mapper
                .Setup(m =>
                    m.Map<IEnumerable<GoverningBodyAdministration>, IEnumerable<GoverningBodyAdministrationDto>>(
                        It.IsAny<IEnumerable<GoverningBodyAdministration>>()))
                .Returns(new List<GoverningBodyAdministrationDto>());
            //Act
            var result = await _governingBodiesService.GetAdministrationForTableAsync(It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>>(result);
        }

        [Test]
        public async Task ContinueGoverningBodyAdminsDueToDateAsync_UpdatesDates()
        {
            //Arrange
            _repoWrapper
                .Setup(g => g.GoverningBodyAdministration.GetAllAsync(
                    It.IsAny<Expression<Func<GoverningBodyAdministration, bool>>>(),
                    It.IsAny<Func<IQueryable<GoverningBodyAdministration>,
                        IIncludableQueryable<GoverningBodyAdministration, object>>>()))
                .ReturnsAsync(GetGoverningBodyAdministrationsForUpdate());

            //Act
            await _governingBodiesService.ContinueGoverningBodyAdminsDueToDateAsync();

            //Assert
            _repoWrapper.Verify();
        }

        private static IEnumerable<GoverningBodyAdministration> GetGoverningBodyAdministrationsForUpdate()
        {
            return new List<GoverningBodyAdministration>
            {
                new GoverningBodyAdministration
                {
                    Id = 1,
                    EndDate = new DateTime(2001, 09, 11, 15, 46, 01),
                    Status = true
                },
                new GoverningBodyAdministration
                {
                    Id = 2,
                    EndDate = new DateTime(2001, 09, 11, 15, 46, 01),
                    Status = true
                },
                new GoverningBodyAdministration
                {
                    Id = 3,
                    EndDate = new DateTime(2001, 09, 11, 15, 46, 01),
                    Status = false
                },
                new GoverningBodyAdministration
                {
                    Id = 4,
                    EndDate = null,
                    Status = true
                }
            }.Where(s => s.Status);
        }

        private IEnumerable<GoverningBodyAdministrationDto> GetTestGoverningBodyAdministration()
        {
            return new List<GoverningBodyAdministrationDto>
            {
                new GoverningBodyAdministrationDto{UserId = Roles.GoverningBodyHead},
                new GoverningBodyAdministrationDto{UserId = Roles.GoverningBodyHead}
            }.AsEnumerable();
        }

        private GoverningBodyDto CreateGoverningBodyDTO => new GoverningBodyDto()
        {
            Id = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = "daa1-4d27-b94d-/9ab2d890d9d0.jpeg,63cc77aa,",
            PhoneNumber = "12345",
            GoverningBodyAdministration = new List<GoverningBodyAdministrationDto>
            {
                new GoverningBodyAdministrationDto
                {
                    AdminType = new AdminTypeDto
                    {
                        AdminTypeName = Roles.GoverningBodyHead
                    }
                },
                new GoverningBodyAdministrationDto
                {
                    AdminType = new AdminTypeDto
                    {
                        AdminTypeName = Roles.GoverningBodySecretary
                    }
                }
            },
            GoverningBodyDocuments = new List<GoverningBodyDocumentsDto>
            {
                new GoverningBodyDocumentsDto()
            },
            IsActive = true
        };

        private List<SectorDto> Sectors => new List<SectorDto>
        {
            new SectorDto(){ Id = 0 },
            new SectorDto(){ Id = 1 },
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
