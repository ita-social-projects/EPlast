using EPlast.BLL.Commands.TermsOfUse;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class ChangeTermsHandler : IRequestHandler<ChangeTermsCommand>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMediator _mediator;

        public ChangeTermsHandler(IRepositoryWrapper repoWrapper, IMediator mediator)
        {
            _repoWrapper = repoWrapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ChangeTermsCommand request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminForTermsQuery(request.User);
            await _mediator.Send(query, cancellationToken);
            var terms = await _repoWrapper.TermsOfUse.GetFirstAsync(x => x.TermsId == request.TermsDto.TermsId);
            terms.TermsTitle = request.TermsDto.TermsTitle;
            terms.TermsText = request.TermsDto.TermsText;
            terms.DatePublication = request.TermsDto.DatePublication;
            _repoWrapper.TermsOfUse.Update(terms);
            await _repoWrapper.SaveAsync();
            return Unit.Value;
        }
    }
}