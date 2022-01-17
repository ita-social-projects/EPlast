using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetClubAdministrationsHandler : IRequestHandler<GetClubAdministrationsQuery, IEnumerable<ClubAdministration>>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public GetClubAdministrationsHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<IEnumerable<ClubAdministration>> Handle(GetClubAdministrationsQuery request, CancellationToken cancellationToken)
        {
            var clubAdminins = await _repoWrapper.ClubAdministration.GetAllAsync(
                                     predicate: c => c.ClubId == request.ClubId && c.Status,
                                     include: source => source
                                      .Include(t => t.AdminType)
                                      .Include(u => u.User).ThenInclude(u => u.CityMembers)
                                                           .ThenInclude(u => u.City)
                                      .Include(d => d.User).ThenInclude(d => d.UserPlastDegrees)
                                                           .ThenInclude(d => d.PlastDegree));
            return clubAdminins;
        }
    }
}
