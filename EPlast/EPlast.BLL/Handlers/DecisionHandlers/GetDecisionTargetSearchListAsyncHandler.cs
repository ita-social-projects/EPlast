using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Queries.Decision;
using EPlast.BLL.DTO;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionTargetSearchListAsyncHandler: IRequestHandler<GetDecisionTargetSearchListAsyncQuery, IEnumerable<DecisionTargetDTO>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetDecisionTargetSearchListAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DecisionTargetDTO>> Handle(GetDecisionTargetSearchListAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DecisionTargetDTO>>((await _repoWrapper.DecesionTarget.GetAllAsync()).Where(d => d.TargetName.Contains(request.searchedData)));
        }
    }
}
