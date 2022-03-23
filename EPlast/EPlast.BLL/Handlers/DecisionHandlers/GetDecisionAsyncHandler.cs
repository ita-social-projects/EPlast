using EPlast.DataAccess.Repositories;
using MediatR;
using EPlast.BLL.Queries.Decision;
using EPlast.BLL.DTO;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Handlers.DecisionHandlers
{
    public class GetDecisionAsyncHandler : IRequestHandler<GetDecisionAsyncQuery, DecisionDTO>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetDecisionAsyncHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<DecisionDTO> Handle(GetDecisionAsyncQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<DecisionDTO>(await _repositoryWrapper.Decesion
                .GetFirstAsync(x => x.ID == request.Id, include: dec =>
                dec.Include(d => d.DecesionTarget).Include(d => d.Organization)));
        }
    }
}
