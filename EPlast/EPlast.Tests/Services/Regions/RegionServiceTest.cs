﻿using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Region;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
        public async Task GetRegionByNameAsync_ReturnsRegionProfileDTO()
        {
            // Arrange
             _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.Region, bool>>>(),
                   It.IsAny<Func<IQueryable<DataAccess.Entities.Region>, IIncludableQueryable<DataAccess.Entities.Region, object>>>()))
                   .ReturnsAsync(new DataAccess.Entities.Region());

            _mapper.Setup(x => x.Map<DataAccess.Entities.Region, RegionProfileDTO>(It.IsAny<DataAccess.Entities.Region>()))
                .Returns(new RegionProfileDTO());

            _userManager.
                Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>() { Roles.admin });
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

            RegionDocumentDTO doc = new RegionDocumentDTO() { ID = 2, BlobName = "Some, book", FileName="Some.FileName" };
            RegionDocuments regionDocuments = new RegionDocuments() { ID = 2, BlobName = "Some, book", FileName = "Some.FileName" };
            _mapper.Setup(x=>x.Map<RegionDocumentDTO, RegionDocuments>(doc))
                .Returns(regionDocuments);
            _repoWrapper.Setup(x=>x.RegionDocument.Attach(regionDocuments));
            // Act
            var result = await _regionService.AddDocumentAsync(doc);
            // Assert
            Assert.IsInstanceOf<RegionDocumentDTO>(result);
            Assert.IsNotNull(result);
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
            Assert.IsInstanceOf<IEnumerable<RegionDocumentDTO>>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DownloadFileAsync_ReturnsString()
        {
            // Arrange
            string fname = "File";
            _regionFilesBlobStorageRepository.Setup(x=>x.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fname);
            // Act
            var result = await _regionService.DownloadFileAsync(It.IsAny<string>());
            // Assert
            Assert.AreEqual(fname,result);
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
        public void DeleteRegionByIdAsync_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(new Region());
            _repoWrapper
                  .Setup(x => x.Region.Delete(new Region()));
            _repoWrapper
                  .Setup(x => x.SaveAsync());
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
                .ReturnsAsync(new Region() { ID=2});

            _repoWrapper.Setup(x => x.City.GetFirstAsync(It.IsAny<Expression<Func<DataAccess.Entities.City, bool>>>(),
                It.IsAny<Func<IQueryable<DataAccess.Entities.City>, IIncludableQueryable<DataAccess.Entities.City, object>>>()))
                .ReturnsAsync(new DataAccess.Entities.City());

            _repoWrapper
                  .Setup(x => x.SaveAsync());
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

            Region reg = new Region() { ID = 2 };
            RegionDTO region = new RegionDTO() { ID = 3, City = "Lviv" };
            _repoWrapper
                   .Setup(x => x.Region.GetFirstAsync(It.IsAny<Expression<Func<Region, bool>>>(),
                It.IsAny<Func<IQueryable<Region>, IIncludableQueryable<Region, object>>>()))
                .ReturnsAsync(reg);

            _repoWrapper
                .Setup(x=>x.Region.Update(reg));

            _repoWrapper
                  .Setup(x => x.SaveAsync());
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

            _regionFilesBlobStorageRepository
                .Setup(x=>x.DeleteBlobAsync(doc.BlobName));

            _repoWrapper
                .Setup(x=>x.RegionDocument.Delete(doc));

            _repoWrapper
                  .Setup(x => x.SaveAsync());
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
            _repoWrapper
               .Setup(x => x.City.Update(It.IsAny<DataAccess.Entities.City>()));
            _repoWrapper
                  .Setup(x => x.SaveAsync());
            // Act
            var result = _regionService.RedirectMembers(It.IsAny<int>(), It.IsAny<int>());
            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void EndAdminsDueToDate_ReturnsCorrect()
        {
            // Arrange
            _repoWrapper
                   .Setup(x => x.RegionAdministration.GetAllAsync(It.IsAny<Expression<Func<RegionAdministration, bool>>>(),
                It.IsAny<Func<IQueryable<RegionAdministration>, IIncludableQueryable<RegionAdministration, object>>>()))
                .ReturnsAsync(Admins);
            _repoWrapper
               .Setup(x => x.RegionAdministration.Update(It.IsAny<RegionAdministration>()));
            _repoWrapper
                  .Setup(x => x.SaveAsync());
            // Act
            var result = _regionService.EndAdminsDueToDate();

            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        [Test]
        public void AddRegionAsync_ReturnsCorrect()
        {
            // Arrange
             _mapper
                .Setup(x => x.Map<RegionDTO, Region>(It.IsAny<RegionDTO>())).Returns(fakeRegion);
            _repoWrapper
                   .Setup(x => x.Region.CreateAsync(fakeRegion));
            _repoWrapper
                  .Setup(x => x.SaveAsync());
            // Act
            var result = _regionService.AddRegionAsync(fakeRegionDTO);
            // Assert
            _repoWrapper.Verify();
            Assert.NotNull(result);
        }

        private readonly Region fakeRegion = new Region()
        {
            RegionName = ""
        };

        private readonly RegionDTO fakeRegionDTO = new RegionDTO
        {
            RegionName = ""
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

        private readonly IEnumerable<DataAccess.Entities.City> city = new List<DataAccess.Entities.City>
        {
            new DataAccess.Entities.City { ID = 1, Name = "Золочів" },
            new DataAccess.Entities.City { ID = 2, Name = "Перемишляни" }
        };

        private readonly IEnumerable<RegionAdministration> Admins = new List<RegionAdministration>
        {
            new RegionAdministration { ID = 1, AdminTypeId = 1 },
            new RegionAdministration { ID = 2, AdminTypeId = 2 }
        };
    }
}
