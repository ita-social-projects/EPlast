using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.City;
using EPlast.BLL.Services.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Regions
{
    class RegionServiceTest
    {

        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IRegionBlobStorageRepository> _regionBlobStorage;
        private Mock<IRegionFilesBlobStorageRepository> _regionFilesBlobStorageRepository;
        private Mock<IMediator> _mediator;
        private Mock<UserManager<User>> _userManager;
        private RegionService _regionService;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _regionBlobStorage = new Mock<IRegionBlobStorageRepository>();
            _regionFilesBlobStorageRepository = new Mock<IRegionFilesBlobStorageRepository>();
            _mediator = new Mock<IMediator>();
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _regionService = new RegionService(
                _repoWrapper.Object,
                _mapper.Object,
                _regionFilesBlobStorageRepository.Object,
                _regionBlobStorage.Object,
                _mediator.Object,
                _userManager.Object
            );
        }

        [Test]
        public async Task ArchiveRegionAsync_ReturnsCorrect()
        {
            // Arrange
            Region reg = new Region()
            {
                ID = 2,
                Cities = null,
                RegionAdministration = null
            };
            IEnumerable<RegionFollowers> followers = new List<RegionFollowers>();
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper
                .Setup(x => x.RegionFollowers.GetAllAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(followers);
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            await _regionService.ArchiveRegionAsync(fakeId);

            // Assert
            _repoWrapper.Verify(r => r.Region.Update(It.IsAny<Region>()), Times.Once);
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public void ArchiveRegionAsync_RegionIsNotEmpty_ThrowsInvalidException()
        {
            // Arrange
            Region reg = new Region()
            {
                ID = 2,
                Cities = new List<EPlast.DataAccess.Entities.City>(),
                RegionAdministration = new List<RegionAdministration>()
            };
            IEnumerable<RegionFollowers> followers = new List<RegionFollowers>
            { 
                new RegionFollowers()
            };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);
            _repoWrapper
                .Setup(x => x.RegionFollowers.GetAllAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
                It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(followers);
            _repoWrapper.Setup(x => x.Region.Update(reg));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _regionService.ArchiveRegionAsync(fakeId));
        }

        [Test]
        public async Task GetAllActiveRegionsAsync_ReturnsIEnumerableActiveRegionDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllActiveRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDto>>(result);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public async Task GetAllRegionsByPageAndIsArchiveAsync_NullInput_ReturnsIEnumerableRegionObjectDTO()
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
            Assert.IsInstanceOf<Tuple<IEnumerable<RegionObjectsDto>, int>>(result);
        }

        [Test]
        public async Task GetAllRegionsAsync_ReturnsIEnumerableRegionDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(x => x.Region.GetAllAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new List<Region>());
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDto>>(result);
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
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionDto>>(It.IsAny<IEnumerable<Region>>()))
                .Returns(regions);

            // Act
            var result = await _regionService.GetAllNotActiveRegionsAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionDto>>(result);
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
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionDto>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(regions.First);

            // Act
            var result = await _regionService.GetRegionByIdAsync(id);
            var actual = result.ID;

            // Assert
            Assert.IsInstanceOf<RegionDto>(result);
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
            _mapper.Setup(x => x.Map<IEnumerable<DataAccess.Entities.City>, IEnumerable<CityDto>>(It.IsAny<IEnumerable<DataAccess.Entities.City>>()))
                .Returns(cities);

            // Act
            var result = await _regionService.GetMembersAsync(2);

            // Assert
            Assert.IsInstanceOf<IEnumerable<CityDto>>(result);
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
                .Setup(x => x.Map<IEnumerable<RegionFollowers>, IEnumerable<RegionFollowerDto>>(It.IsAny<IEnumerable<RegionFollowers>>()))
                .Returns(regionFollowers);

            // Act
            var result = await _regionService.GetFollowersAsync(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionFollowerDto>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetFollowerAsync_ReturnsRegionFollowerDTO()
        {
            // Arrange
            RegionFollowerDto regionFollower = new RegionFollowerDto();
            _repoWrapper.Setup(x => x.RegionFollowers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<RegionFollowers, bool>>>(),
              It.IsAny<Func<IQueryable<RegionFollowers>, IIncludableQueryable<RegionFollowers, object>>>()))
                .ReturnsAsync(new RegionFollowers());
            _mapper
                .Setup(x => x.Map<RegionFollowers, RegionFollowerDto>(It.IsAny<RegionFollowers>()))
                .Returns(regionFollower);

            // Act
            var result = await _regionService.GetFollowerAsync(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<RegionFollowerDto>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreateFollowerAsync_ReturnsFollowerId()
        {
            // Arrange
            int id = 1;

            _mapper
               .Setup(x => x.Map<RegionFollowerDto, RegionFollowers>(It.IsAny<RegionFollowerDto>()))
               .Returns(new RegionFollowers() { ID = id });
            _repoWrapper.Setup(x => x.RegionFollowers.CreateAsync(new RegionFollowers()));
            _repoWrapper.Setup(x => x.SaveAsync());

            // Act
            var result = _regionService.CreateFollowerAsync(It.IsAny<RegionFollowerDto>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Result);
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
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionProfileDto>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(new RegionProfileDto());
            _userManager.
                Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.Admin, Roles.OkrugaHead, Roles.OkrugaHeadDeputy });

            // Act
            var result = await _regionService.GetRegionByNameAsync(It.IsAny<string>(), It.IsAny<User>());

            // Assert
            Assert.IsInstanceOf<RegionProfileDto>(result);
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
            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionDto>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(new RegionDto());

            // Act
            var result = await _regionService.GetRegionByNameAsync(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOf<RegionDto>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AddDocumentAsync_ReturnsRegionDocumentDTO()
        {
            // Arrange
            RegionDocumentDto doc = new RegionDocumentDto() { ID = 2, BlobName = "Some, book", FileName = "Some.doc" };
            RegionDocuments regionDocuments = new RegionDocuments() { ID = 2, BlobName = "Some, book", FileName = "Some.doc" };
            _mapper.Setup(x => x.Map<RegionDocumentDto, RegionDocuments>(doc))
                .Returns(regionDocuments);
            _repoWrapper.Setup(x => x.RegionDocument.Attach(regionDocuments));

            // Act
            var result = await _regionService.AddDocumentAsync(doc);

            // Assert
            Assert.IsInstanceOf<RegionDocumentDto>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddDocumentAsync_DocumentHasNoExtension_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDto doc = new RegionDocumentDto() { ID = 2, BlobName = "Some, book", FileName = "Name" };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _regionService.AddDocumentAsync(doc));
        }

        [Test]
        public void AddDocumentAsync_DocumentNameIsEmpty_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDto doc = new RegionDocumentDto() { ID = 2, BlobName = "Some, book", FileName = ".doc" };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _regionService.AddDocumentAsync(doc));
        }

        [Test]
        public void AddDocumentAsync_DocumentHasWrongExtension_ThrowsArgumentExeption()
        {
            // Arrange
            RegionDocumentDto doc = new RegionDocumentDto() { ID = 2, BlobName = "Some, book", FileName = "qwe.dejavu" };

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

            _mapper.Setup(x => x.Map<IEnumerable<RegionDocuments>, IEnumerable<RegionDocumentDto>>(It.IsAny<List<RegionDocuments>>()))
                .Returns(documentDTOs);

            // Act
            var result = await _regionService.GetRegionDocsAsync(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionDocumentDto>>(result);
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
            _mapper.Setup(x => x.Map<IEnumerable<Region>, IEnumerable<RegionForAdministrationDto>>(It.IsAny<List<Region>>()))
                .Returns(regionsForAdmin);

            // Act
            var result = await _regionService.GetRegions();

            // Assert
            Assert.IsInstanceOf<IEnumerable<RegionForAdministrationDto>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetActiveRegionsNames_ReturnsIEnumerableRegionNamesDTO()
        {
            // Arrange
            _repoWrapper.Setup(x => x.Region.GetActiveRegionsNames());
            _mapper
                .Setup(x => x.Map<IEnumerable<RegionNamesObject>, IEnumerable<RegionNamesDto>>(It.IsAny<List<RegionNamesObject>>()))
                .Returns(regionsNames);

            // Act
            var result = _regionService.GetActiveRegionsNames();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<RegionNamesDto>>(result);
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
            RegionDto region = new RegionDto()
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
            RegionDto region = new RegionDto() { ID = 3, City = "Lviv", Logo = string.Empty };
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
            _mediator.Setup(x => x.Send(It.IsAny<GetCitiesByRegionQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(cities);
            _mapper
                .Setup(x => x.Map<RegionDto, RegionProfileDto>(It.IsAny<RegionDto>()))
                .Returns(regionProfileDTO);
            _mapper
                .Setup(x => x.Map<Region, RegionDto>(It.IsAny<Region>()))
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
            Assert.IsInstanceOf<RegionProfileDto>(result);
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
               .Setup(x => x.Map<RegionDto, Region>(It.IsAny<RegionDto>())).Returns(fakeRegion);
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
            Assert.IsInstanceOf<Task<IEnumerable<RegionUserDto>>>(result);
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
                .Setup(m => m.Map<RegionDto>(It.IsAny<Region>()))
                .Returns(new RegionDto());

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
                .Setup(m => m.Map<RegionDto>(It.IsAny<Region>()))
                .Returns(nullRegionDTO);

            //Act
            var result = await _regionService.CheckIfRegionNameExistsAsync(It.IsAny<string>());

            //Assert
            Assert.IsFalse(result);
        }

        private readonly int fakeId = 6;

        private readonly User user = new User();

        private readonly Region nullRegion = null;

        private readonly RegionDto nullRegionDTO = null;


        private readonly Region fakeRegion = new Region()
        {
            RegionName = ""
        };

        private readonly RegionDto fakeRegionDTO = new RegionDto
        {
            RegionName = ""
        };

        private readonly RegionProfileDto regionProfileDTO = new RegionProfileDto
        {
            Cities = new List<CityDto>()
        };

        private readonly RegionDto regionDTO = new RegionDto
        {
            City = "city",
            Logo = "Some logo.png"
        };

        private readonly IEnumerable<RegionNamesDto> regionsNames = new List<RegionNamesDto>
        {
            new RegionNamesDto { ID = 1, RegionName = "Львівський" },
            new RegionNamesDto { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<RegionForAdministrationDto> regionsForAdmin = new List<RegionForAdministrationDto>
        {
            new RegionForAdministrationDto { ID = 1, RegionName = "Львівський" },
            new RegionForAdministrationDto { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<RegionDocumentDto> documentDTOs = new List<RegionDocumentDto>
        {
            new RegionDocumentDto { ID = 1, BlobName="Some, name" },
            new RegionDocumentDto { ID = 2, BlobName="Some, 2name" }
        };

        private readonly IEnumerable<RegionDto> regions = new List<RegionDto>
        {
            new RegionDto { ID = 1, RegionName = "Львівський" },
            new RegionDto { ID = 2, RegionName = "Тернопільський" }
        };

        private readonly IEnumerable<CityDto> cities = new List<CityDto>
        {
            new CityDto { ID = 1, Name = "Золочів" },
            new CityDto { ID = 2, Name = "Перемишляни" }
        };

        private readonly IEnumerable<RegionFollowerDto> regionFollowers = new List<RegionFollowerDto>
        {
            new RegionFollowerDto { ID = 1, CityName = "Золочів" },
            new RegionFollowerDto { ID = 2, CityName = "Перемишляни" }
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
