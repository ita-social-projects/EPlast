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

    }
}
