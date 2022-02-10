using EPlast.BLL.Commands.Precaution;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class ChangePrecautionHandler: IRequestHandler<ChangePrecautionCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMediator _mediator;

        public ChangePrecautionHandler(IRepositoryWrapper repositoryWrapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ChangePrecautionCommand request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query, cancellationToken);

            var precaution = await _repositoryWrapper.Precaution.GetFirstAsync(x => x.Id == request.PrecautionDTO.Id);
            precaution.Name = request.PrecautionDTO.Name;
            _repositoryWrapper.Precaution.Update(precaution);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
