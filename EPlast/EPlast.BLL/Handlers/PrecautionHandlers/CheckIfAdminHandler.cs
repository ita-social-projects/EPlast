using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class CheckIfAdminHandler: IRequestHandler<CheckIfAdminQuery>
    {
        private readonly UserManager<User> _userManager;

        public CheckIfAdminHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(CheckIfAdminQuery request, CancellationToken cancellationToken)
        {
            if (!(await _userManager.GetRolesAsync(request.User)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();

            return Unit.Value;
        }
    }
}
