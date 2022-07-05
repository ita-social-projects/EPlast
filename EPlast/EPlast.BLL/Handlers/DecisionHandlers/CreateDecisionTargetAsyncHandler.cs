using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class CreateDecisionTargetAsyncHandler : IRequestHandler<CreateDecisionTargetAsyncCommand, DecisionTargetDto>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public CreateDecisionTargetAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<DecisionTargetDto> Handle(CreateDecisionTargetAsyncCommand request, CancellationToken cancellationToken)
        {
            DecisionTargetDto decisionTargetDto = _mapper.Map<DecisionTargetDto>(await _repoWrapper.DecesionTarget.GetFirstOrDefaultAsync(x => x.TargetName == request.DecisionTargetName));

            if (decisionTargetDto == null)
            {
                decisionTargetDto = new DecisionTargetDto();
                decisionTargetDto.TargetName = request.DecisionTargetName;
            }

            return decisionTargetDto;
        }
    }
}
