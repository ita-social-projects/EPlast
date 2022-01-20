using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class GetAllUsersIdWithoutSenderHandler : IRequestHandler<GetAllUsersIdWithoutSenderQuery, IEnumerable<string>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public GetAllUsersIdWithoutSenderHandler(IRepositoryWrapper repoWrapper, IMediator mediator, UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> Handle(GetAllUsersIdWithoutSenderQuery request, CancellationToken cancellationToken)
        {
            var query = new CheckIfAdminForTermsQuery(request.User);
            await _mediator.Send(query, cancellationToken);
            var userId = await _userManager.GetUserIdAsync(request.User);
            var allUsersId = (await _repoWrapper.UserProfile.GetAllAsync(x => x.UserID != userId))
                .Select(c => c.UserID);
            return allUsersId;
        }
    }
}