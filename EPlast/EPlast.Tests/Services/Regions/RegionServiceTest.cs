using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Region;
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

namespace EPlast.Tests.Services.Regions
{
    class RegionServiceTest
    {

        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IRegionBlobStorageRepository> _regionBlobStorage;
        private Mock<IRegionFilesBlobStorageRepository> _regionFilesBlobStorageRepository;
        private Mock<ICityService> _cityService;
        private Mock<IUniqueIdService> _uniqueId;
        private Mock<UserManager<User>> _userManager;
        private RegionService _regionService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _regionBlobStorage = new Mock<IRegionBlobStorageRepository>();
            _regionFilesBlobStorageRepository = new Mock<IRegionFilesBlobStorageRepository>();
            _cityService = new Mock<ICityService>();
            _uniqueId = new Mock<IUniqueIdService>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _regionService = new RegionService(
                _repoWrapper.Object, _mapper.Object, _regionFilesBlobStorageRepository.Object,
                _regionBlobStorage.Object, _cityService.Object, _uniqueId.Object, _userManager.Object
                );
        }

        [Test]
        public void ArchiveRegionAsync_ReturnsCorrect()
        {
            // Arrange
            Region reg = new Region() { ID = 2 };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            _ = _regionService.ArchiveRegionAsync(fakeId);

            // Assert
            _repoWrapper.Verify(r => r.Region.Update(It.IsAny<Region>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllActiveRegionsAsync_ReturnsIEnumerableActiveRegionDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDTO>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllActiveRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDTO>>(result);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public async Task UsersTableAsync_NullInput_ReturnsIEnumerableUserTableDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetRegionsObjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(CreateTuple);
            _regionBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).Throws(new ArgumentException("Can not get image"));

            // Act
            var result = await _regionService.GetAllRegionsByPageAndIsArchiveAsync(1, 2, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Tuple<IEnumerable<RegionObjectsDTO>, int>>(result);
        }

        [Test]
        public async Task GetAllRegionsAsync_ReturnsIEnumerableRegionDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDTO>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllNotActiveRegionsAsync_ReturnsIEnumerableNotActiveRegionDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDTO>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllNotActiveRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDTO>>(result);
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task GetLogoBase64_ReturnsStringLogo()
        {
            // Arrange
            string logo = "logo";
            _regionBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(logo);

            // Act
            var result = await _regionService.GetLogoBase64(logo);

            // Assert
            Assert.IsInstanceOf<string>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRegionByIdAsync_ReturnsRegionDTO()
        {
            // Arrange
            int id = 1;
            _repoWrapper
                .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.Region());
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionDTO>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(regions.First);

            // Act
            var result = await _regionService.GetRegionByIdAsync(id);
            var actual = result.ID;

            // Assert
            Assert.IsInstanceOf<RegionDTO>(result);
            Assert.AreEqual(1, actual);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetMembersAsync_ReturnsIEnumerableCityDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
              It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new List<DataAccess.Entities.City>());
            _mapper.Setup(x => x.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDTO>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(cities);

            // Act
            var result = await _regionService.GetMembersAsync(2);

            // Assert
            Assert.IsInstanceOf<IEnumerable<CityDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetFollowersAsync_ReturnsIEnumerableRegionFollowerDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.RegionFollowers.GetAllAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(new List<RegionFollowers>());
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionFollowers>, IEnumerable<RegionFollowerDTO>>(It.IsAny<IEnumerable<RegionFollowers>>()))
                .Returns(regionFollowers);

            // Act
            var result = await _regionService.GetFollowersAsync(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionFollowerDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetFollowerAsync_ReturnsRegionFollowerDTO()
        {
            // Arrange
            RegionFollowerDTO regionFollower = new RegionFollowerDTO();
            _repoWrapper.Setup(x => x.RegionFollowers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
              It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(new RegionFollowers());
            _mapper
                .Setup(x => x.Map<RegionFollowers, RegionFollowerDTO>(It.IsAny<RegionFollowers>()))
                .Returns(regionFollower);

            // Act
            var result = await _regionService.GetFollowerAsync(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<RegionFollowerDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreateFollowerAsync_ReturnsSuccess()
        {
            // Arrange
            _mapper
               .Setup(x => x.Map<RegionFollowerDTO, RegionFollowers>(It.IsAny<RegionFollowerDTO>()))
               .Returns(new RegionFollowers());
            _repoWrapper.Setup(x => x.RegionFollowers.CreateAsync(new RegionFollowers()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.CreateFollowerAsync(It.IsAny<RegionFollowerDTO>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void RemoveFollowerAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.RegionFollowers
                   .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                   It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                   .ReturnsAsync(It.IsAny<RegionFollowers>());
            _repoWrapper.Setup(x => x.RegionFollowers.Delete(It.IsAny<RegionFollowers>()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.RemoveFollowerAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetRegionByNameAsync_ReturnsRegionProfileDTO()
        {
            // Arrange
            _repoWrapper
                  .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>()))
                  .ReturnsAsync(new DataAccess.Entities.Region());
            _repoWrapper
                .Setup(x => x.RegionDocument.GetAllAsync(It.IsAny<Expression<Func<RegionDocuments, bool>>>(),
              It.IsAny<Func<IQueryable<RegionDocuments>, IIncludableQueryable<RegionDocuments, object>>>()))
                .ReturnsAsync(new List<RegionDocuments> { new RegionDocuments() });
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionProfileDTO>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(new RegionProfileDTO());
            _userManager.
                Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.OkrugaHead, Roles.OkrugaHeadDeputy });

            // Act
            var result = await _regionService.GetRegionByNameAsync(It.IsAny<string>(), It.IsAny<User>());

            // Assert
            Assert.IsInstanceOf<RegionProfileDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRegionByNameAsync_ReturnsRegionDTO()
        {
            // Arrange
            _repoWrapper
                  .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                  It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>()))
                  .ReturnsAsync(new DataAccess.Entities.Region());
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionDTO>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(new RegionDTO());

            // Act
            var result = await _regionService.GetRegionByNameAsync(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOf<RegionDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsRegionDocumentDTO()
        {
            // Arrange
            RegionDocumentDTO doc = new RegionDocumentDTO() { ID = 2, BlobName = "Some, book", FileName = "Some.doc" };
            RegionDocuments regionDocuments = new RegionDocuments() { ID = 2, BlobName = "Some, book", FileName = "Some.doc" };
            _mapper.Setup(x => x.Map<RegionDocumentDTO, RegionDocuments>(doc))
                .Returns(regionDocuments);
            _repoWrapper.Setup(x => x.RegionDocument.Attach(regionDocuments));

            // Act
            var result = await _regionService.AddDocumentAsync(doc);

            // Assert
            Assert.IsInstanceOf<RegionDocumentDTO>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddDocumentAsync_DocumentHasNoExtension_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDTO doc = new RegionDocumentDTO() { ID = 2, BlobName = "Some, book", FileName = "Name" };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _regionService.AddDocumentAsync(doc));
        }

        [Test]
        public void AddDocumentAsync_DocumentNameIsEmpty_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDTO doc = new RegionDocumentDTO() { ID = 2, BlobName = "Some, book", FileName = ".doc" };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _regionService.AddDocumentAsync(doc));
        }

        [Test]
        public void AddDocumentAsync_DocumentHasWrongExtension_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDTO doc = new RegionDocumentDTO() { ID = 2, BlobName = "Some, book", FileName = "qwe.dejavu" };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _regionService.AddDocumentAsync(doc));
        }

        [Test]
        public async Task GetRegionDocsAsync_ReturnsIEnumerableRegionDocumentDTO()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.RegionDocument.GetAllAsync(It.IsAny<Expression<Func<RegionDocuments, bool>>>(),
                   It.IsAny<Func<IQueryable<RegionDocuments>, IIncludableQueryable<RegionDocuments, object>>>()))
                   .ReturnsAsync(new List<RegionDocuments>());

            _mapper.Setup(x => x.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDTO>>(It.IsAny<List<RegionDocuments>>()))
                .Returns(documentDTOs);

            // Act
            var result = await _regionService.GetRegionDocsAsync(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionDocumentDTO>>(result);
        }

        [Test]
        public async Task DownloadFileAsync_ReturnsString()
        {
            // Arrange
            string fname = "File";
            _regionFilesBlobStorageRepository.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fname);

            // Act
            var result = await _regionService.DownloadFileAsync(It.IsAny<string>());

            // Assert
            Assert.AreEqual(fname, result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRegions_ReturnsIEnumerableRegionForAdministrationDTO()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionForAdministrationDTO>>(It.IsAny<List<Region>>()))
                .Returns(regionsForAdmin);

            // Act
            var result = await _regionService.GetRegions();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionForAdministrationDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetActiveRegionsNames_ReturnsIEnumerableRegionNamesDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.Region.GetActiveRegionsNames());
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionNamesObject>, IEnumerable<RegionNamesDTO>>(It.IsAny<List<RegionNamesObject>>()))
                .Returns(regionsNames);

            // Act
            var result = _regionService.GetActiveRegionsNames();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionNamesDTO>>(result);
        }

        [Test]
        public void DeleteRegionByIdAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new Region());
            _repoWrapper.Setup(x => x.Region.Delete(new Region()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.DeleteRegionByIdAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void AddFollowerAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new Region() { ID = 2 });

            _repoWrapper.Setup(x => x.City.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City());
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.AddFollowerAsync(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }


        [Test]
        public void EditRegionAsync_ReturnsCorrect()
        {
            // Arrange
            Region reg = new Region() { ID = 2, Logo = "some logo" };
            RegionDTO region = new RegionDTO()
            {
                ID = 3,
                City = "Lviv",
                Logo = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD//gA7Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c"
            };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper
                   .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _regionBlobStorage.Setup(x => x.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _regionBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());
            // Act
            var result = _regionService.EditRegionAsync(It.IsAny<int>(), region);

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void EditRegionAsync_LogoEmpty_ReturnsCorrect()
        {
            // Arrange
            Region reg = new Region() { ID = 2, Logo = "some logo" };
            RegionDTO region = new RegionDTO() { ID = 3, City = "Lviv", Logo = string.Empty };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper
                   .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.EditRegionAsync(It.IsAny<int>(), region);

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteFileAsync_ReturnsCorrect()
        {
            // Arrange
            RegionDocuments doc = new RegionDocuments() { ID = 2, BlobName = "blobName" };
            _repoWrapper
                   .Setup(x => x.RegionDocument
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionDocuments, bool>>>(),
                It.IsAny<Func<IQueryable<RegionDocuments>, IIncludableQueryable<RegionDocuments, object>>>()))
                .ReturnsAsync(doc);
            _regionFilesBlobStorageRepository.Setup(x => x.DeleteBlobAsync(doc.BlobName));
            _repoWrapper.Setup(x => x.RegionDocument.Delete(doc));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.DeleteFileAsync(It.IsAny<int>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void RedirectMembers_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.City.GetAllAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(city);
            _repoWrapper.Setup(x => x.City.Update(It.IsAny<DataAccess.Entities.City>()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.RedirectMembers(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetRegionProfileByIdAsync_ReturnsCorrectAsync()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new Region());
            _cityService
                .Setup(x => x.GetCitiesByRegionAsync(It.IsAny<int>()))
                .ReturnsAsync(cities);
            _mapper
                .Setup(x => x.Map<RegionDTO, RegionProfileDTO>(It.IsAny<RegionDTO>()))
                .Returns(regionProfileDTO);
            _mapper
                .Setup(x => x.Map<Region, RegionDTO>(It.IsAny<Region>()))
                .Returns(regionDTO);
            _userManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { Roles.Admin, Roles.OkrugaHead, Roles.OkrugaHeadDeputy });
            _repoWrapper
                .Setup(x => x.RegionDocument.GetAllAsync(It.IsAny<Expression<Func<RegionDocuments, bool>>>(),
            It.IsAny<Func<IQueryable<RegionDocuments>, IIncludableQueryable<RegionDocuments, object>>>()))
                .ReturnsAsync(new List<RegionDocuments>());
            _regionBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>())).ThrowsAsync(new Exception("Can not get blob"));

            // Act
            var result = await _regionService.GetRegionProfileByIdAsync(fakeId, user);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RegionProfileDTO>(result);
        }

        [Test]
        public void ContinueAdminsDueToDate_EndDateEarlier_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
                  .ReturnsAsync(new List<RegionAdministration> { new RegionAdministration() { ID = fakeId, EndDate = new DateTime(2001, 7, 20) } });
            _repoWrapper.Setup(x => x.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.ContinueAdminsDueToDate();

            // Assert
            _repoWrapper.Verify(x => x.SaveAsync());
            _repoWrapper.Verify(x => x.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            Assert.NotNull(result);
        }

        [Test]
        public void ContinueAdminsDueToDate_EndDateNull_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
                  .ReturnsAsync(new List<RegionAdministration> { new RegionAdministration() { ID = fakeId } });
            _repoWrapper.Setup(x => x.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.ContinueAdminsDueToDate();

            // Assert
            _repoWrapper.Verify(x => x.SaveAsync());
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAsync_ReturnsCorrect()
        {
            // Arrange
            _mapper
               .Setup(x => x.Map<RegionDTO, Region>(It.IsAny<RegionDTO>())).Returns(fakeRegion);
            _repoWrapper.Setup(x => x.Region.CreateAsync(fakeRegion));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.AddRegionAsync(fakeRegionDTO);

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }


        [Test]
        public void GetRegionUsersAsync_ReturnDTO()
        {
            // Arrange
            int regionID = 1;
            _repoWrapper
                   .Setup(x => x.CityMembers.GetAllAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>, IIncludableQueryable<CityMembers, object>>>()))
                  .ReturnsAsync(new List<CityMembers> { new CityMembers() });

            // Act
            var result = _regionService.GetRegionUsersAsync(regionID);

            // Assert
            Assert.NotNull(result);
            _repoWrapper.Verify();
            Assert.IsInstanceOf<Task<IEnumerable<RegionUserDTO>>>(result);
        }

        [Test]
        public void UnArchivRegionAsync_ReturnsCorrect()
        {
            // Arrange
            Region reg = new Region() { ID = 2 };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            _ = _regionService.UnArchiveRegionAsync(fakeId);

            // Assert
            _repoWrapper.Verify(r => r.Region.Update(It.IsAny<Region>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task CheckIfRegionNameExistsAsync_ReturnsTrue()
        {
            //Arrange
            _repoWrapper
               .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new Region());
            _mapper
                .Setup(m => m.Map<RegionDTO>(It.IsAny<Region>()))
                .Returns(new RegionDTO());

            //Act
            var result = await _regionService.CheckIfRegionNameExistsAsync(It.IsAny<string>());

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckIfRegionNameExistsAsync_ReturnsFalse()
        {
            //Arrange
            _repoWrapper
               .Setup(x => x.Region.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(nullRegion);
            _mapper
                .Setup(m => m.Map<RegionDTO>(It.IsAny<Region>()))
                .Returns(nullRegionDTO);

            //Act
            var result = await _regionService.CheckIfRegionNameExistsAsync(It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        private readonly int fakeId = 6;

        private readonly User user = new User();

        private readonly Region nullRegion = null;

        private readonly RegionDTO nullRegionDTO = null;


        private readonly Region fakeRegion = new Region()
        {
            RegionName = ""
        };

        private readonly RegionDTO fakeRegionDTO = new RegionDTO
        {
            RegionName = ""
        };

        private readonly RegionProfileDTO regionProfileDTO = new RegionProfileDTO
        {
            Cities = new List<CityDTO>()
        };

        private readonly RegionDTO regionDTO = new RegionDTO
        {
            City = "city",
            Logo = "Some logo.png"
        };

        private readonly IEnumerable<RegionNamesDTO> regionsNames = new List<RegionNamesDTO>
        {
            new RegionNamesDTO { ID = 1, RegionName = "Львівський" },
            new RegionNamesDTO { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<RegionForAdministrationDTO> regionsForAdmin = new List<RegionForAdministrationDTO>
        {
            new RegionForAdministrationDTO { ID = 1, RegionName = "Львівський" },
            new RegionForAdministrationDTO { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<RegionDocumentDTO> documentDTOs = new List<RegionDocumentDTO>
        {
            new RegionDocumentDTO { ID = 1, BlobName="Some, name" },
            new RegionDocumentDTO { ID = 2, BlobName="Some, 2name" }
        };

        private readonly IEnumerable<RegionDTO> regions = new List<RegionDTO>
        {
            new RegionDTO { ID = 1, RegionName = "Львівський" },
            new RegionDTO { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<CityDTO> cities = new List<CityDTO>
        {
            new CityDTO { ID = 1, Name = "Золочів" },
            new CityDTO { ID = 2, Name = "Перемишляни" }
        };

        private readonly IEnumerable<RegionFollowerDTO> regionFollowers = new List<RegionFollowerDTO>
        {
            new RegionFollowerDTO { ID = 1, CityName = "Золочів" },
            new RegionFollowerDTO { ID = 2, CityName = "Перемишляни" }
        };

        private readonly IEnumerable<DataAccess.Entities.City> city = new List<DataAccess.Entities.City>
        {
            new DataAccess.Entities.City { ID = 1, Name = "Золочів" },
            new DataAccess.Entities.City { ID = 2, Name = "Перемишляни" }
        };

        private Tuple<IEnumerable<RegionObject>, int> CreateTuple => new Tuple<IEnumerable<RegionObject>, int>(CreateRegionObjects, 100);

        private IEnumerable<RegionObject> CreateRegionObjects => new List<RegionObject>()
        {
            new RegionObject(){ Logo = "logo.png"},
            new RegionObject()
        };
    }

}
