using EPlast.BLL.Commands.Decision;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
namespace EPlast.Tests.Handlers.Decision
{
    public class UploadFileToBlobAsyncHandlerTest
    {
        private Mock<IDecisionBlobStorageRepository> _blob;
        private UploadFileToBlobAsyncHandler _handler;
        private UploadFileToBlobAsyncCommand _query;
        [SetUp]
        public void SetUp()
        {
            _blob = new Mock<IDecisionBlobStorageRepository>();
            _handler = new UploadFileToBlobAsyncHandler(_blob.Object);
        }
        [Test]
        public async Task UploadFileToBlobAsync()
        {
            //Arrange
            _blob.Setup(x => x.UploadBlobForBase64Async(It.IsAny<string>(), It.IsAny<string>()));
            _query = new UploadFileToBlobAsyncCommand(It.IsAny<string>(), It.IsAny<string>());

            //Act
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsInstanceOf<Unit>(actualReturn);

        }
    }
}
