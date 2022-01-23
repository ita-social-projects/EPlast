using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Queries.City;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class GetCityLogoBase64HandlerTest
    {
        private Mock<ICityBlobStorageRepository> _mockBlobStorage;
        private GetCityLogoBase64Query _query;
        private GetCityLogoBase64Handler _handler;

        private const string CityName = "CityName";

        [SetUp]
        public void SetUp()
        {
            _mockBlobStorage = new Mock<ICityBlobStorageRepository>();
            _query = new GetCityLogoBase64Query(CityName);
            _handler = new GetCityLogoBase64Handler(_mockBlobStorage.Object);
        }

        [Test]
        public async Task GetCityLogoBase64Test_ReturnsLogo()
        {
            //Arrange
            _mockBlobStorage
                .Setup(b => b.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(CityName);

            //Act
            var result = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }
    }
}
