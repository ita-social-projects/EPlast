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
    public class GetClubHistoryMembersHandler : IRequestHandler<GetClubHistoryMembersQuery, IEnumerable<ClubMemberHistoryDto>>
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GetClubHistoryMembersHandler(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubMemberHistoryDto>> Handle(GetClubHistoryMembersQuery request, CancellationToken cancellationToken)
        {
            var clubHistoryMembers = await _repoWrapper.ClubMemberHistory.GetAllAsync(
                                          predicate: c => c.ClubId == request.ClubId &&
                                                     !c.IsFollower &&
                                                     !c.IsDeleted,
                                          include: source => source
                                                     .Include(x => x.User).ThenInclude(x => x.UserPlastDegrees)
                                                                          .ThenInclude(x => x.PlastDegree)
                                                     .Include(d => d.User).ThenInclude(c => c.CityMembers)
                                                                          .ThenInclude(c => c.City));

            return _mapper.Map<IEnumerable<ClubMemberHistory>, IEnumerable<ClubMemberHistoryDto>>(clubHistoryMembers);
        }
    }
}
