using System.Threading;
using System.Threading.Tasks;
using EPlast.BLL.Queries.City;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Handlers.CityHandlers
{
    public class PlastMemberCheckHandler : IRequestHandler<PlastMemberCheckQuery, bool>
    {
        private readonly UserManager<User> _userManager;

        public PlastMemberCheckHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(PlastMemberCheckQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            return await _userManager.IsInRoleAsync(user, Roles.PlastMember);
        }
    }
}
