using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Queries.Decision;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionTargetSearchListAsyncHandler : IRequestHandler<GetDecisionTargetSearchListAsyncQuery, IEnumerable<DecisionTargetDto>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetDecisionTargetSearchListAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DecisionTargetDto>> Handle(GetDecisionTargetSearchListAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DecisionTargetDto>>((await _repoWrapper.DecesionTarget.GetAllAsync()).Where(d => d.TargetName.Contains(request.SearchedData)));
        }
    }
}
