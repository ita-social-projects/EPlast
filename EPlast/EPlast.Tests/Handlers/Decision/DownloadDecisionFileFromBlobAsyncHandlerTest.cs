using EPlast.BLL.Queries.Decision;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Decision
{
    public class DownloadDecisionFileFromBlobAsyncHandlerTest
    {
        private Mock<IDecisionBlobStorageRepository> _blob;
        private DownloadDecisionFileFromBlobAsyncHandler _handler;
        private DownloadDecisionFileFromBlobAsyncQuery _query;
        [SetUp]
        public void SetUp()
        {
            _blob = new Mock<IDecisionBlobStorageRepository>();
            _handler = new DownloadDecisionFileFromBlobAsyncHandler(_blob.Object);
        }
        [TestCase("filename1")]
        [TestCase("filename2")]
        public async Task DownloadDecisionFileFromBlobAsyncTest(string fileName)
        {
            //Arrange
            _blob.Setup(blobStorage => blobStorage.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fileName);
            _query = new DownloadDecisionFileFromBlobAsyncQuery(fileName);
            //Act
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(fileName, actualReturn);
        }
    }
}
