using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class ChangeDistinctionHandler : IRequestHandler<ChangeDistinctionQuery>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ChangeDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ChangeDistinctionQuery request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query);

            var distinction = _mapper.Map<DistinctionDTO, Distinction>(request.DistinctionDTO);
            await _repositoryWrapper.Distinction.CreateAsync(distinction);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
