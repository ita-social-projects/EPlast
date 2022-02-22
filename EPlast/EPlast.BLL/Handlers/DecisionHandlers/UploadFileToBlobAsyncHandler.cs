using System.Threading;
using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Commands.Decision;
using AutoMapper;
using EPlast.BLL.DTO;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.AzureStorage;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class UploadFileToBlobAsyncHandler : IRequestHandler<UploadFileToBlobAsyncCommand>
    {
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;
        public UploadFileToBlobAsyncHandler(IDecisionBlobStorageRepository decisionBlobStorage)
        {
            _decisionBlobStorage = decisionBlobStorage;
        }

        public async Task<Unit> Handle(UploadFileToBlobAsyncCommand request, CancellationToken cancellationToken)
        {
            await _decisionBlobStorage.UploadBlobForBase64Async(request.base64, request.fileName);

            return Unit.Value;
        }
    }
}
