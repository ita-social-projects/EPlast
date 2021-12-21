using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetCountDeletedUsersPerYearHandler : IRequestHandler<GetCountDeletedUsersPerYearQuery, int>
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public GetCountDeletedUsersPerYearHandler(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<int> Handle(GetCountDeletedUsersPerYearQuery request, CancellationToken cancellationToken)
        {
            var deletedUsersPerYear = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                     predicate: c => c.ClubId == request.ClubId && !c.IsFollower &&
                                                c.IsDeleted && c.Date.Year == DateTime.Now.Year);

            return deletedUsersPerYear.Count();
        }
    }
}
