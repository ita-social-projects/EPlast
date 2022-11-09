using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Distinction;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class AddDistinctionHandler : IRequestHandler<AddDistinctionCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AddDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddDistinctionCommand request, CancellationToken cancellationToken)
        {
            var distinction = _mapper.Map<DistinctionDto, Distinction>(request.DistinctionDTO);
            await _repositoryWrapper.Distinction.CreateAsync(distinction);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
