using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class AddDistinctionHandler : IRequestHandler<AddDistinctionQuery>
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

        public async Task<Unit> Handle(AddDistinctionQuery request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query);

            var distinction = await _repositoryWrapper.Distinction.GetFirstAsync(x => x.Id == request.DistinctionDTO.Id);
            distinction.Name = request.DistinctionDTO.Name;
            _repositoryWrapper.Distinction.Update(distinction);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
