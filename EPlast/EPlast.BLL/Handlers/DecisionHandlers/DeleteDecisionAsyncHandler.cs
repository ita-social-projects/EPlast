using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Commands.Decision;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using EPlast.DataAccess.Entities;
using System;
using EPlast.BLL.Interfaces.AzureStorage;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class DeleteDecisionAsyncHandler:IRequestHandler<DeleteDecisionAsyncCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IDecisionBlobStorageRepository _decisionBlobStorage;

        public DeleteDecisionAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IDecisionBlobStorageRepository decisionBlobStorage)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _decisionBlobStorage = decisionBlobStorage;
        }

        public async Task<Unit> Handle(DeleteDecisionAsyncCommand request, CancellationToken cancellationToken)
        {
            var decision = (await _repoWrapper.Decesion.GetFirstAsync(d => d.ID == request.Id));
            if (decision == null)
                throw new ArgumentNullException($"Decision with {request.Id} id not found");
            _repoWrapper.Decesion.Delete(decision);
            if (decision.FileName != null)
                await _decisionBlobStorage.DeleteBlobAsync(decision.FileName);
            await _repoWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
