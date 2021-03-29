using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;

namespace EPlast.Tests.Services
{
    class GoverningBodiesServiceTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private GoverningBodiesService _service;
        private Mock<IUniqueIdService> _uniqueIdService;
        private Mock<IGoverningBodyBlobStorageRepository> _blobStorage;
        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobStorage = new Mock<IGoverningBodyBlobStorageRepository>();
            _uniqueIdService = new Mock<IUniqueIdService>();
            _service = new GoverningBodiesService(
                _repoWrapper.Object,
                _mapper.Object,
                _uniqueIdService.Object,
                _blobStorage.Object);
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
            var result = await _service.GetGoverningBodiesListAsync();
            //Assert
            Assert.IsInstanceOf<IEnumerable<GoverningBodyDTO>>(result);
        }

        [Test]
        public async Task CreateAsync_Test()
        {
            //Arrange
            var testDTO = CreateGoverningBodyDTO;
            _mapper
                .Setup(x => x.Map<Organization>(It.IsAny<GoverningBodyDTO>())).Returns(new Organization() {ID = testDTO.ID, Logo = testDTO.Logo });
            _mapper
                .Setup(x => x.Map<GoverningBodyDTO, Organization>(It.IsAny<GoverningBodyDTO>()))
                .Returns(_mapper.Object.Map<Organization>(testDTO));
            _repoWrapper
                .Setup(x => x.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                    It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>()))
                .ReturnsAsync(_mapper.Object.Map<Organization>(testDTO));

            //Act
            var result = await _service.CreateAsync(testDTO);

            //Assert
            Assert.AreEqual(testDTO.ID, result);
            _repoWrapper.Verify(x => x.GoverningBody.CreateAsync(_mapper.Object.Map<Organization>(testDTO)), Times.Once);
            _repoWrapper.Verify(x => x.SaveAsync(), Times.Once);
        }

        private GoverningBodyDTO CreateGoverningBodyDTO => new GoverningBodyDTO()
        {
            ID = 1,
            GoverningBodyName = "gbName",
            Description = "gbDesc",
            Email = "gbEmail",
            Logo = "daa1-4d27-b94d-/9ab2d890d9d0.jpeg,63cc77aa,",
            PhoneNumber = "12345"
        };
    }
}
