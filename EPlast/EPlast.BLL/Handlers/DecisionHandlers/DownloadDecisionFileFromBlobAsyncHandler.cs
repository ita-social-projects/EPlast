using MediatR;
using EPlast.BLL.DTO;
using AutoMapper;
using EPlast.DataAccess.Repositories;
using System.Threading.Tasks;
using System.Threading;
using EPlast.BLL.Queries.Decision;
using EPlast.BLL.Interfaces.AzureStorage;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class DownloadDecisionFileFromBlobAsyncHandler : IRequestHandler<DownloadDecisionFileFromBlobAsyncQuery, string>
    {
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;
        public DownloadDecisionFileFromBlobAsyncHandler(IDecisionBlobStorageRepository decisionBlobStorage)
        {
            _decisionBlobStorage = decisionBlobStorage;
        }

        public async Task<string> Handle(DownloadDecisionFileFromBlobAsyncQuery request, CancellationToken cancellationToken)
        {
            return await _decisionBlobStorage.GetBlobBase64Async(request.FileName);
        }
    }
}
