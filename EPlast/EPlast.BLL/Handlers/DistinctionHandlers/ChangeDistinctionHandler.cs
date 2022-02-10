using EPlast.BLL.Commands.Distinction;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class ChangeDistinctionHandler : IRequestHandler<ChangeDistinctionCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMediator _mediator;

        public ChangeDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ChangeDistinctionCommand request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query, cancellationToken);

            var distinction = await _repositoryWrapper.Distinction.GetFirstAsync(x => x.Id == request.DistinctionDTO.Id);
            distinction.Name = request.DistinctionDTO.Name;
            _repositoryWrapper.Distinction.Update(distinction);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
