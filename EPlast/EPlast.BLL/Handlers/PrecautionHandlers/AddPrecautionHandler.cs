using AutoMapper;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class AddPrecautionHandler: IRequestHandler<AddPrecautionCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AddPrecautionHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddPrecautionCommand request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminQuery(request.User);
            await _mediator.Send(query, cancellationToken);

            var precaution = _mapper.Map<PrecautionDTO, Precaution>(request.PrecautionDTO);
            await _repositoryWrapper.Precaution.CreateAsync(precaution);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
