using AutoMapper;
using EPlast.BLL.Commands.TermsOfUse;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class AddTermsHandler : IRequestHandler<AddTermsCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AddTermsHandler(IRepositoryWrapper repoWrapper , IMapper mapper , IMediator mediator)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddTermsCommand request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminForTermsQuery(request.User);
            await _mediator.Send(query, cancellationToken);
            var terms = _mapper.Map<TermsDTO, DataAccess.Entities.Terms>(request.TermsDto);
            await _repoWrapper.TermsOfUse.CreateAsync(terms);
            await _repoWrapper.SaveAsync();
            return Unit.Value;
        }
    }
}