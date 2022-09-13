using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Queries.City;
using EPlast.Resources;
using MediatR;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityAdminsIdsHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private GetCityAdminsIdsHandler _handler;
        private GetCityAdminsIdsQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _handler = new GetCityAdminsIdsHandler(_mockMediator.Object);
            _query = new GetCityAdminsIdsQuery(CityId);
        }

        [Test]
        public async Task GetCityAdminsIdsHandlerTest_ReturnsAdminsIds()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCity());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual("UserId,No Id", result);
        }

        [Test]
        public async Task GetCityAdminsIdsHandlerTest_ReturnsNull()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNull(result);
        }

        private static CityDto GetCity()
        {
            return new CityDto
            {
                CityAdministration = new List<CityAdministrationDto>
                {
                    new CityAdministrationDto
                    {
                        AdminType = new AdminTypeDto
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        UserId = "UserId",
                        Status = true
                    }
                },
                CityMembers = new List<CityMembersDto>
                {
                    new CityMembersDto
                    {
                        IsApproved = true
                    }
                },
                CityDocuments = new List<CityDocumentsDto>
                {
                    new CityDocumentsDto
                    {
                        ID = 1
                    }
                }
            };
        }
    }
}
