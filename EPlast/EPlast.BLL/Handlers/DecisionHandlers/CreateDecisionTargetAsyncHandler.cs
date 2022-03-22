using System.Threading.Tasks;
using System.Threading;
using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Commands.Decision;
using AutoMapper;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class CreateDecisionTargetAsyncHandler : IRequestHandler<CreateDecisionTargetAsyncCommand, DecisionTargetDTO>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        
        public CreateDecisionTargetAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<DecisionTargetDTO> Handle(CreateDecisionTargetAsyncCommand request, CancellationToken cancellationToken)
        {
            DecisionTargetDTO decisionTargetDto = _mapper.Map<DecisionTargetDTO>(await _repoWrapper.DecesionTarget.GetFirstOrDefaultAsync(x => x.TargetName == request.DecisionTargetName));

            if (decisionTargetDto == null)
            {
                decisionTargetDto = new DecisionTargetDTO();
                decisionTargetDto.TargetName = request.DecisionTargetName;
            }

            return decisionTargetDto;
        }
    }
}
