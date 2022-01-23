using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Commands.City;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Handlers.CityHandlers;
using EPlast.DataAccess.Repositories;
using MediatR;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.City
{
    public class EditCityHandlerTest
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IRepositoryWrapper> _mockRepoWrapper;
        private EditCityCommand _command;
        private EditCityHandler _handler;

        private CityDTO _city;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _city = new CityDTO { ID = 1, Region = new RegionDTO { ID = 1, RegionName = "Region" } };
            _command = new EditCityCommand(_city);
            _handler = new EditCityHandler(_mockMediator.Object, _mockRepoWrapper.Object);
        }

        [Test]
        public async Task EditCityHandlerTest_EditsCity()
        {
            //Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<UploadCityPhotoCommand>(), It.IsAny<CancellationToken>()));
            _mockMediator
                .Setup(m => m.Send(It.IsAny<CreateCityCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataAccess.Entities.City());
            _mockRepoWrapper
                .Setup(r => r.City.Attach(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.City.Update(It.IsAny<DataAccess.Entities.City>()));
            _mockRepoWrapper
                .Setup(r => r.SaveAsync());
            //Act
            var result = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Unit>(result);
        }
    }
}
