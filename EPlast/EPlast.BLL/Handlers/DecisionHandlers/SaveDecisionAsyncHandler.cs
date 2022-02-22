using System.Threading.Tasks;
using System.Threading;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Commands.Decision;
using AutoMapper;
using EPlast.BLL.Interfaces;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class SaveDecisionAsyncHandler: IRequestHandler<SaveDecisionAsyncCommand, int>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IUniqueIdService _uniqueId;
        private readonly IMediator _mediator;
        public SaveDecisionAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IUniqueIdService uniqueId, IMediator mediator)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _uniqueId = uniqueId;
            _mediator = mediator;
        }

        public async Task<int> Handle(SaveDecisionAsyncCommand request, CancellationToken cancellationToken)
        {
            var query = new CreateDecisionTargetAsyncCommand(request.decision.Decision.DecisionTarget.TargetName);
            request.decision.Decision.DecisionTarget = await _mediator.Send(query);
            var repoDecision = _mapper.Map<Decesion>(request.decision.Decision);
            _repoWrapper.Decesion.Attach(repoDecision);
            _repoWrapper.Decesion.Create(repoDecision);
            if (request.decision.FileAsBase64 != null)
            {
                repoDecision.FileName = $"{_uniqueId.GetUniqueId()}{repoDecision.FileName}";
                var uploadFileToBlobAsync = new UploadFileToBlobAsyncCommand(request.decision.FileAsBase64, repoDecision.FileName);
                await _mediator.Send(uploadFileToBlobAsync);
            }
            await _repoWrapper.SaveAsync();

            return request.decision.Decision.ID;
        }
    }
}
