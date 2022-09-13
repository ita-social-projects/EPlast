using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class CreateCityWthIdHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private CreateCityWthIdCommand _command;
        private CreateCityWthIdHandler _handler;

        private CityDto _city;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _city = new CityDto { ID = 1 };
            _command = new CreateCityWthIdCommand(_city);
            _handler = new CreateCityWthIdHandler(_mockMediator.Object, _mockRepoWrapper.Object);
        }

        [Test]
        public async Task CreateCityWthIdHandlerTest_CreatesCity()
        {
            //Arrange
            var city = new DataAccess.Entities.City {ID = 1};
            _mockMediator
                .Setup(m => m.Send(It.IsAny<UploadCityPhotoCommand>(), It.IsAny<CancellationToken>()));
            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateCityCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(city);
            _mockRepoWrapper
                .Setup(r => r.City.Attach(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.City.CreateAsync(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.SaveAsync());

            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
        }
    }
}
