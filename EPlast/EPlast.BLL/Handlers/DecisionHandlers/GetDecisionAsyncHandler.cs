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
    public class GetDecisionAsyncHandler : IRequestHandler<GetDecisionAsyncQuery, DecisionDto>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDecisionAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<DecisionDto> Handle(GetDecisionAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<DecisionDto>(await _repositoryWrapper.Decesion
                .GetFirstAsync(x => x.ID == request.Id, include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization)));
        }
    }
}
