using EPlast.BLL.Queries.Decision;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.BLL.Interfaces.AzureStorage;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest.Handlers.Decision
{
    public class DownloadDecisionFileFromBlobAsyncHandlerTest
    {
        private Mock<IDecisionBlobStorageRepository> _blob;
        private DownloadDecisionFileFromBlobAsyncHandler _handler;
        private DownloadDecisionFileFromBlobAsyncQuery _query;
       
        public DownloadDecisionFileFromBlobAsyncHandlerTest()
        {
            _blob = new Mock<IDecisionBlobStorageRepository>();
            _handler = new DownloadDecisionFileFromBlobAsyncHandler(_blob.Object);
        }

        [Theory]
        [InlineData("filename1")]
        [InlineData("filename2")]
        public async Task DownloadDecisionFileFromBlobAsyncTest(string fileName)
        {
            _blob.Setup(blobStorage => blobStorage.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync(fileName);

            _query = new DownloadDecisionFileFromBlobAsyncQuery(fileName);
            var actualReturn = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            Assert.Equal(fileName, actualReturn);
        }
    }
}
