using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class DeletePrecautionHandler: IRequestHandler<DeletePrecautionQuery>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMediator _mediator;

        public DeletePrecautionHandler(IRepositoryWrapper repositoryWrapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeletePrecautionQuery request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query, cancellationToken);

            var precaution = (await _repositoryWrapper.Precaution.GetFirstAsync(d => d.Id == request.Id));
            if (precaution == null)
                throw new ArgumentNullException($"Precaution with {request.Id} not found");
            _repositoryWrapper.Precaution.Delete(precaution);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
