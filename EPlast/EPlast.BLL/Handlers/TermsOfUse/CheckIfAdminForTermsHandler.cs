using System;
using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Handlers.TermsOfUse
{
    public class CheckIfAdminForTermsHandler : IRequestHandler<CheckIfAdminForTermsQuery>
    {
        private readonly UserManager<User> _userManager;

        public CheckIfAdminForTermsHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(CheckIfAdminForTermsQuery request, CancellationToken cancellationToken)
        {
            if (!(await _userManager.GetRolesAsync(request.User)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
            return Unit.Value;
        }
    }
}