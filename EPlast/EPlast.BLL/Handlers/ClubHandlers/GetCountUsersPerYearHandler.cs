using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetCountUsersPerYearHandler : IRequestHandler<GetCountUsersPerYearQuery, int>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public GetCountUsersPerYearHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> Handle(GetCountUsersPerYearQuery request, CancellationToken cancellationToken)
        {
            var usersPerYear = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                        predicate: c => c.ClubId == request.ClubId &&
                                                   !c.IsFollower && c.Date.Year == DateTime.Now.Year);

            return usersPerYear.Count();
        }
    }
}
