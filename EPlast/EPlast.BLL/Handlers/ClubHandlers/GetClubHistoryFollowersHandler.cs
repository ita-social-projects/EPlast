using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Handlers.ClubHandlers
{
    public class GetClubHistoryFollowersHandler : IRequestHandler<GetClubHistoryFollowersQuery, IEnumerable<ClubMemberHistoryDto>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetClubHistoryFollowersHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubMemberHistoryDto>> Handle(GetClubHistoryFollowersQuery request, CancellationToken cancellationToken)
        {
            var clubHistoryFollowers = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                              predicate: c => c.ClubId == request.ClubId &&
                                                         c.IsFollower &&
                                                         !c.IsDeleted,
                                               include: source => source
                                                      .Include(x => x.User).ThenInclude(x => x.UserPlastDegrees)
                                                                           .ThenInclude(x => x.PlastDegree)
                                                      .Include(d => d.User).ThenInclude(c => c.CityMembers)
                                                                           .ThenInclude(c => c.City));

            return _mapper.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDto>>(clubHistoryFollowers);
        }
    }
}
