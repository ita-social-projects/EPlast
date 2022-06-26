using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class SaveDecisionAsyncHandler: IRequestHandler<SaveDecisionAsyncCommand, int>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SaveDecisionAsyncHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IMediator mediator
        )
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> Handle(SaveDecisionAsyncCommand request, CancellationToken cancellationToken)
        {
            var query = new CreateDecisionTargetAsyncCommand(request.Decision.Decision.DecisionTarget.TargetName);
            request.Decision.Decision.DecisionTarget = await _mediator.Send(query);
            var repoDecision = _mapper.Map<Decesion>(request.Decision.Decision);
            _repoWrapper.Decesion.Attach(repoDecision);
            _repoWrapper.Decesion.Create(repoDecision);
            if (request.Decision.FileAsBase64 != null)
            {
                repoDecision.FileName = $"{Guid.NewGuid()}{repoDecision.FileName}";
                var uploadFileToBlobAsync = new UploadFileToBlobAsyncCommand(request.Decision.FileAsBase64, repoDecision.FileName);
                await _mediator.Send(uploadFileToBlobAsync);
            }
            await _repoWrapper.SaveAsync();

            return request.Decision.Decision.ID;
        }
    }
}
