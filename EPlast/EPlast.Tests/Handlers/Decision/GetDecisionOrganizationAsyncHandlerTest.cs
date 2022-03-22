using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Decision
{
    public class GetDecisionOrganizationAsyncHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private GetDecisionOrganizationAsyncHandler _handler;
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDecisionOrganizationAsyncHandler(_repository.Object, _mockMapper.Object);
        }
        [TestCase(null)]
        [TestCase("")]
        public async Task GetDecisionOrganizationAsyncWithEmptyOrNullParameterTest(string organizationName)
        {
            //Arrange
            GoverningBodyDTO governingBody = GetTestOrganizationDtoList()[0];
            governingBody.GoverningBodyName = organizationName;
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { ID = governingBody.Id });
            _mockMapper.Setup(m => m.Map<GoverningBodyDTO>(string.IsNullOrEmpty(It.IsAny<string>()))).Returns(governingBody);
                
            //Act
            var query = new GetDecisionOrganizationAsyncQuery(governingBody);
            var actualReturn = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(governingBody.Id, actualReturn.Id);
        }

        [Test]
        public async Task GetDecisionOrganizationAsyncWithRightParameterTest()
        {
            //Arrange
            GoverningBodyDTO organization = GetTestOrganizationDtoList()[0];
            _repository.Setup(rep => rep.GoverningBody.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Organization, bool>>>(),
                It.IsAny<Func<IQueryable<Organization>, IIncludableQueryable<Organization, object>>>())).ReturnsAsync(new Organization() { OrganizationName = organization.GoverningBodyName });
            _mockMapper.Setup(m => m.Map<GoverningBodyDTO>(string.IsNullOrEmpty(It.IsAny<string>()))).Returns(organization);
            //Act
            var query = new GetDecisionOrganizationAsyncQuery(organization);
            var actualReturn = await _handler.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(organization.GoverningBodyName, actualReturn.GoverningBodyName);
        }
        private static List<GoverningBodyDTO> GetTestOrganizationDtoList()
        {
            return new List<GoverningBodyDTO>
            {
                new GoverningBodyDTO {Id = 1,GoverningBodyName = "Organization1"},
                new GoverningBodyDTO {Id = 2,GoverningBodyName = "Organization2"},
            };
        }

    }
}
