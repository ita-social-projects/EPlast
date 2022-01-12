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
    public class GetCityDocumentsHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private GetCityDocumentsHandler _handler;
        private GetCityDocumentsQuery _query;

        private const int CityId = 1;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _handler = new GetCityDocumentsHandler(_mockMediator.Object);
            _query = new GetCityDocumentsQuery(CityId);
        }

        [Test]
        public async Task GetCityDocumentsHandlerTest_ReturnsCityProfileWthDocuments()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdWthFullInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCity());

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CityProfileDTO>(result);
        }

        [Test]
        public async Task GetCityDocumentsHandlerTest_ReturnsNull()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetCityByIdWthFullInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNull(result);
        }

        private static CityDTO GetCity()
        {
            return new CityDTO
            {
                CityAdministration = new List<CityAdministrationDTO>
                {
                    new CityAdministrationDTO
                    {
                        AdminType = new AdminTypeDTO
                        {
                            AdminTypeName = Roles.CityHead
                        },
                        UserId = "UserId",
                        Status = true
                    }
                },
                CityMembers = new List<CityMembersDTO>
                {
                    new CityMembersDTO
                    {
                        IsApproved = true
                    }
                },
                CityDocuments = new List<CityDocumentsDTO>
                {
                    new CityDocumentsDTO
                    {
                        ID = 1
                    }
                }
            };
        }
    }
}
